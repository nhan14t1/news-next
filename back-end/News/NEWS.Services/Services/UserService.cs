using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NEWS.Entities.Constants;
using NEWS.Entities.Exceptions;
using NEWS.Entities.Extensions;
using NEWS.Entities.Models.Dto;
using NEWS.Entities.Models.ViewModels;
using NEWS.Entities.MySqlEntities;
using NEWS.Entities.Repositories;
using NEWS.Entities.Services;
using NEWS.Entities.UnitOfWorks;
using NEWS.Entities.Utils;

namespace NEWS.Services.Services
{
    public class UserService : BaseService<User>, IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IMapper _mapper;

        public UserService(IUnitOfWork unitOfWork, IUserRepository userRepository,
            IRoleRepository roleRepository, IUserRoleRepository userRoleRepository,
            IMapper mapper) : base(unitOfWork)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _userRoleRepository = userRoleRepository;
            _mapper = mapper;
        }

        public async Task<bool> ValidateUserAsync(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                return false;
            }

            var user = await _userRepository.GetByEmailAsync(email);

            if (user == null || !user.IsActive)
            {
                return false;
            }

            return user.Password == CryptoUtils.SHA256Crypt(password, user.Salt);
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await _userRepository.GetByEmailAsync(email);
        }

        public async Task CreateUserAsync(string email, string password, string firstName,
            string lastName, int age, string phoneNumber, bool isAdmin = false)
        {
            var user = new User
            {
                Email = email,
                Password = password,
                FirstName = firstName,
                LastName = lastName,
                Age = age,
                PhoneNumber = phoneNumber,
                IsActive = true,
            };

            await CreateUserAsync(user, isAdmin);
        }

        public async Task CreateUserAsync(User user, bool isAdmin = false)
        {
            if (string.IsNullOrWhiteSpace(user.Email) || string.IsNullOrWhiteSpace(user.Password))
            {
                throw new BusinessException("Email và mật khẩu không được trống");
            }

            var salt = CryptoUtils.GenerateBase64Salt();
            var password = CryptoUtils.SHA256Crypt(user.Password, salt);

            var newUser = new User
            {
                Email = user.Email.ToLower(),
                Salt = salt,
                Password = password,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Age = user.Age,
                PhoneNumber = user.PhoneNumber,
                IsActive = true,
                CreatedDate = DateTime.Now.ToFileTime(),
            };

            try
            {
                await _unitOfWork.BeginTransaction();
                _unitOfWork.DbContext.Add(newUser);
                await _unitOfWork.SaveChangesAsync();
                _unitOfWork.DbContext.Add(new UserRole
                {
                    UserId = newUser.Id,
                    RoleId = (int)AppRoles.User,
                });

                if (isAdmin)
                {
                    _unitOfWork.DbContext.Add(new UserRole
                    {
                        UserId = newUser.Id,
                        RoleId = (int)AppRoles.Admin,
                    });
                }

                await _unitOfWork.Commit();
            }
            catch (Exception e)
            {
                await _unitOfWork.RollBack();
                throw;
            }
        }

        private void ValidateCreation(UserVM user, bool isEdit = false)
        {
            if (string.IsNullOrWhiteSpace(user.Email) || (!isEdit && string.IsNullOrWhiteSpace(user.Password)))
            {
                throw new BusinessException("Email và mật khẩu không được trống");
            }

            if (user.RoleIds == null || !user.RoleIds.Any())
            {
                throw new BusinessException("Vui lòng chọn chức vụ");
            }
        }

        public async Task<UserDto> CreateUserAsync(UserVM user)
        {
            ValidateCreation(user);
            var salt = CryptoUtils.GenerateBase64Salt();
            var password = CryptoUtils.SHA256Crypt(user.Password, salt);

            var newUser = new User
            {
                Email = user.Email.ToLower(),
                Salt = salt,
                Password = password,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Age = user.Age,
                PhoneNumber = user.PhoneNumber,
                IsActive = true,
                CreatedDate = DateTime.Now.ToTimeStamp(),
            };

            try
            {
                await _unitOfWork.BeginTransaction();
                _unitOfWork.DbContext.Add(newUser);
                await _unitOfWork.SaveChangesAsync();

                foreach (var roleId in user.RoleIds)
                {
                    _unitOfWork.DbContext.Add(new UserRole
                    {
                        UserId = newUser.Id,
                        RoleId = roleId,
                    });
                }

                await _unitOfWork.Commit();
            }
            catch (Exception e)
            {
                await _unitOfWork.RollBack();
                throw;
            }

            return _mapper.Map<UserDto>(newUser);
        }

        public async Task<UserDto> UpdateUserAsync(UserVM request)
        {
            ValidateCreation(request, true);

            // Avoid updating the user email.
            var user = await _repository.GetAll(_ => _.Id == request.Id
                && _.Email == request.Email)
                .Include(_ => _.UserRoles)
                .ThenInclude(_ => _.Role)
                .FirstOrDefaultAsync();

            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.Age = request.Age;
            user.PhoneNumber = request.PhoneNumber;


            // Update password also
            if (!string.IsNullOrWhiteSpace(request.Password))
            {
                user.Salt = CryptoUtils.GenerateBase64Salt();
                user.Password = CryptoUtils.SHA256Crypt(request.Password, user.Salt);
            }

            var oldRoles = user.UserRoles;
            var newRoleIds = request.RoleIds;

            var createRoleIds = newRoleIds.Where(roleId => !oldRoles.Any(oldRole => oldRole.RoleId == roleId)).ToList();
            var deleteRoles = oldRoles.Where(role => !newRoleIds.Any(id => id == role.RoleId)).ToList();

            try
            {
                await _unitOfWork.BeginTransaction();
                _unitOfWork.DbContext.Update(user);

                foreach (var roleId in createRoleIds)
                {
                    _unitOfWork.DbContext.Add(new UserRole
                    {
                        UserId = user.Id,
                        RoleId = roleId,
                    });
                }

                if (deleteRoles.Any())
                {
                    _unitOfWork.DbContext.RemoveRange(deleteRoles);
                }

                await _unitOfWork.Commit();
            }
            catch (Exception e)
            {
                await _unitOfWork.RollBack();
                throw;
            }

            return _mapper.Map<UserDto>(user);
        }

        public async Task<List<UserDto>> GetAllAsync()
        {
            var users = await _repository.GetAll(_ => true)
                .Include(_ => _.UserRoles)
                .OrderByDescending(_ => _.Id)
                .ToListAsync();

            return _mapper.Map<List<UserDto>>(users);
        }

        public async Task DeactivateAsync(int userId)
        {
            var user = await _repository.GetAll(_ => _.Id == userId)
                .FirstOrDefaultAsync();

            if (user.Email == "admin")
            {
                throw new BusinessException("Không thể hủy user 'admin'!");
            }

            user.IsActive = false;
            await _unitOfWork.SaveChangesAsync();

            // Block token
        }

        public async Task ActivateAsync(int userId)
        {
            var user = await _repository.GetAll(_ => _.Id == userId)
                .FirstOrDefaultAsync();

            user.IsActive = true;
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task ChangePasswordAsync(string email, string newPass)
        {
            var user = await _repository.GetAll(_ => _.Email == email).FirstOrDefaultAsync();
            var salt = CryptoUtils.GenerateBase64Salt();
            var password = CryptoUtils.SHA256Crypt(newPass, salt);
            user.Password = password;
            user.Salt = salt;

            await _repository.UpdateAsync(user);
        }
    }
}

using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NEWS.Entities.Constants;
using NEWS.Entities.Exceptions;
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

        public async Task<UserDto> CreateUserAsync(UserVM user)
        {
            if (string.IsNullOrWhiteSpace(user.Email) || string.IsNullOrWhiteSpace(user.Password))
            {
                throw new BusinessException("Email và mật khẩu không được trống");
            }

            if (user.RoleId == 0)
            {
                throw new BusinessException("Vui lòng chọn chức vụ");
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
                IsActive = user.IsActive,
            };

            try
            {
                await _unitOfWork.BeginTransaction();
                _unitOfWork.DbContext.Add(newUser);
                await _unitOfWork.SaveChangesAsync();
                _unitOfWork.DbContext.Add(new UserRole
                {
                    UserId = newUser.Id,
                    RoleId = user.RoleId,
                });

                await _unitOfWork.Commit();
            }
            catch (Exception e)
            {
                await _unitOfWork.RollBack();
                throw;
            }

            return _mapper.Map<UserDto>(newUser);
        }

        public async Task CreateUserAsync(User user, bool isAdmin = false)
        {
            if (string.IsNullOrWhiteSpace(user.Email))
            {
                throw new Exception();
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
                IsActive = user.IsActive,
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

        public async Task<List<UserDto>> GetAllAsync()
        {
            var users = await _repository.GetAll(_ => true)
                .OrderByDescending(_ => _.Id)
                .ToListAsync();

            return _mapper.Map<List<UserDto>>(users);
        }

    }
}

using NEWS.Entities.Constants;
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
        public UserService(IUnitOfWork unitOfWork, IUserRepository userRepository,
            IRoleRepository roleRepository, IUserRoleRepository userRoleRepository) : base(unitOfWork)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _userRoleRepository = userRoleRepository;
        }

        public async Task<bool> ValidateUserAsync(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                return false;
            }

            var user = await _userRepository.GetByEmailAsync(email);

            if (user == null)
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
            };

            await CreateUserAsync(user, isAdmin);
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
                PhoneNumber = user.PhoneNumber
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
    }
}

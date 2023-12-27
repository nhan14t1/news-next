using NEWS.Entities.Models.Dto;
using NEWS.Entities.Models.ViewModels;
using NEWS.Entities.MySqlEntities;

namespace NEWS.Entities.Services
{
    public interface IUserService : IBaseService<User>
    {
        Task<bool> ValidateUserAsync(string email, string password);

        Task<User> GetByEmailAsync(string email);

        Task CreateUserAsync(User user, bool isAdmin = false);

        Task CreateUserAsync(string email, string password, string firstName,
            string lastName, int age, string phoneNumber, bool isAdmin = false);

        Task<UserDto> CreateUserAsync(UserVM user);

        Task<UserDto> UpdateUserAsync(UserVM user);

        Task<List<UserDto>> GetAllAsync();
    }
}

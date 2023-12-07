using NEWS.Entities.MySqlEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NEWS.Entities.Services
{
    public interface IUserService : IBaseService<User>
    {
        Task<bool> ValidateUserAsync(string email, string password);
        Task<User> GetByEmailAsync(string email);
        Task CreateUserAsync(User user, bool isAdmin = false);
        Task CreateUserAsync(string email, string password, string firstName,
            string lastName, int age, string phoneNumber, bool isAdmin = false);
    }
}

using NEWS.Entities.MySqlEntities;

namespace NEWS.Entities.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> GetByEmailAsync(string email);
    }
}

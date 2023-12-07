using NEWS.Entities.MySqlEntities;

namespace NEWS.Entities.Services
{
    public interface IUserRoleService : IBaseService<UserRole>
    {
        Task<List<Role>> GetRolesByEmailAsync(string email);
    }
}

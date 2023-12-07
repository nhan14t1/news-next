using NEWS.Entities.MySqlEntities;

namespace NEWS.Entities.Services
{
    public interface IRoleService : IBaseService<Role>
    {
        List<Role> GetByEmail(string email);
    }
}

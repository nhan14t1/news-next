using Microsoft.EntityFrameworkCore;
using NEWS.Entities.MySqlEntities;
using NEWS.Entities.Repositories;

namespace NEWS.Repositories.Repositories
{
    public class UserRoleRepository : Repository<UserRole>, IUserRoleRepository
    {
        public UserRoleRepository(DbContext dbContext) : base(dbContext) { }
    }
}

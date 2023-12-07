using Microsoft.EntityFrameworkCore;
using NEWS.Entities.MySqlEntities;
using NEWS.Entities.Repositories;

namespace NEWS.Repositories.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(DbContext dbContext) : base(dbContext) { }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await Entities.Include(_ => _.UserRoles)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Email == email);
        }
    }
}

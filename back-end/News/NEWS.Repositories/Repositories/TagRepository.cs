using Microsoft.EntityFrameworkCore;
using NEWS.Entities.MySqlEntities;
using NEWS.Entities.Repositories;

namespace NEWS.Repositories.Repositories
{
    public class TagRepository : Repository<Tag>, ITagRepository
    {
        public TagRepository(DbContext dbContext) : base(dbContext) { }
    }
}

using Microsoft.EntityFrameworkCore;
using NEWS.Entities.MySqlEntities;
using NEWS.Entities.Repositories;

namespace NEWS.Repositories.Repositories
{
    public class PostTagRepository : Repository<PostTag>, IPostTagRepository
    {
        public PostTagRepository(DbContext dbContext) : base(dbContext) { }
    }
}

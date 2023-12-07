using Microsoft.EntityFrameworkCore;
using NEWS.Entities.MySqlEntities;
using NEWS.Entities.Repositories;

namespace NEWS.Repositories.Repositories
{
    public class PostRepository : Repository<Post>, IPostRepository
    {
        public PostRepository(DbContext dbContext) : base(dbContext) { }
    }
}

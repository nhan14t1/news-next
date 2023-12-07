using Microsoft.EntityFrameworkCore;
using NEWS.Entities.MySqlEntities;
using NEWS.Entities.Repositories;

namespace NEWS.Repositories.Repositories
{
    public class PostCategoryRepository : Repository<PostCategory>, IPostCategoryRepository
    {
        public PostCategoryRepository(DbContext dbContext) : base(dbContext) { }
    }
}

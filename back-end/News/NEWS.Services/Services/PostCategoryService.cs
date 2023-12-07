using NEWS.Entities.MySqlEntities;
using NEWS.Entities.Services;
using NEWS.Entities.UnitOfWorks;

namespace NEWS.Services.Services
{
    public class PostCategoryService : BaseService<PostCategory>, IPostCategoryService
    {
        public PostCategoryService(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        
        }
    }
}

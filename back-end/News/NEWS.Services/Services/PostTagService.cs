using NEWS.Entities.MySqlEntities;
using NEWS.Entities.Services;
using NEWS.Entities.UnitOfWorks;

namespace NEWS.Services.Services
{
    public class PostTagService : BaseService<PostTag>, IPostTagService
    {
        public PostTagService(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }
    }
}

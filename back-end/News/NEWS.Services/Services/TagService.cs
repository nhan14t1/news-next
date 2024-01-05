using NEWS.Entities.MySqlEntities;
using NEWS.Entities.Services;
using NEWS.Entities.UnitOfWorks;

namespace NEWS.Services.Services
{
    public class TagService : BaseService<Tag>, ITagService
    {
        public TagService(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }
    }
}

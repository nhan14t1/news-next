using NEWS.Entities.MySqlEntities;
using NEWS.Entities.Services;
using NEWS.Entities.UnitOfWorks;

namespace NEWS.Services.Services
{
    public class CategoryService : BaseService<Category>, ICategoryService
    {
        public CategoryService(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }
    }
}

using Microsoft.EntityFrameworkCore;
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

        public async Task<List<Tag>> SearchAsync(string keyword)
        {
            keyword = keyword.ToLower();
            var result = await _repository.GetAll(_ => _.LowerText.Contains(keyword))
                .AsNoTracking().ToListAsync();

            return result;
        }
    }
}

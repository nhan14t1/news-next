using Microsoft.EntityFrameworkCore;
using NEWS.Entities.MySqlEntities;
using NEWS.Entities.Services;
using NEWS.Entities.UnitOfWorks;

namespace NEWS.Services.Services
{
    public class RoleService : BaseService<Role>, IRoleService
    {
        public RoleService(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        public List<Role> GetByEmail(string email)
        {
            return _repository.GetAll(_ => _.UserRoles.FirstOrDefault(x => x.User.Email == email) != null)
                .AsNoTracking()
                .ToList();
        }
    }
}

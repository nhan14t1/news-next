using Microsoft.EntityFrameworkCore;
using NEWS.Entities.MySqlEntities;
using NEWS.Entities.Repositories;
using NEWS.Entities.Services;
using NEWS.Entities.UnitOfWorks;

namespace NEWS.Services.Services
{
    public class UserRoleService : BaseService<UserRole>, IUserRoleService
    {
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;

        public UserRoleService(IUnitOfWork unitOfWork, IUserRoleRepository userRoleRepository,
            IUserRepository userRepository, IRoleRepository roleRepository)
            : base(unitOfWork)
        {
            _userRoleRepository = userRoleRepository;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
        }

        public async Task<List<Role>> GetRolesByEmailAsync(string email)
        {
            throw new NotImplementedException();
            return await _roleRepository.Entities.Include(_ => _.UserRoles)
                .ThenInclude(_ => _.Role)
                .ToListAsync();
        }
    }
}

using Microsoft.EntityFrameworkCore;
using NEWS.Entities.MySqlEntities;
using NEWS.Entities.Repositories;

namespace NEWS.Repositories.Repositories
{
    public class FileManagementRepository : Repository<FileManagement>, IFileManagementRepository
    {
        public FileManagementRepository(DbContext context) : base(context)
        {

        }
    }
}

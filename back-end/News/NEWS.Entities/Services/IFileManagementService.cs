using NEWS.Entities.Constants;
using NEWS.Entities.MySqlEntities;

namespace NEWS.Entities.Services
{
    public interface IFileManagementService : IBaseService<FileManagement>
    {
        Task<FileManagement> Add(string fileName, string fileExtension, FileType type,
            DateTime? createdDate = null, bool isUsed = false);

        Task<FileManagement> AddImages(string fileName, string fileExtension);
        
        Task<FileManagement> AddThumbnail(string fileName, string fileExtension);
    }
}

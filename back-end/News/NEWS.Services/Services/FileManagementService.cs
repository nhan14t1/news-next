using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NEWS.Entities.Constants;
using NEWS.Entities.Extensions;
using NEWS.Entities.MySqlEntities;
using NEWS.Entities.Services;
using NEWS.Entities.UnitOfWorks;

namespace NEWS.Services.Services
{
    public class FileManagementService : BaseService<FileManagement>, IFileManagementService
    {
        private readonly ILogger _logger;
        public FileManagementService(IUnitOfWork unitOfWork,
            ILogger<FileManagementService> logger)
            : base(unitOfWork)
        {
            _logger = logger;
        }

        public async Task<FileManagement> Add(string fileName, string fileExtension, FileType type,
            DateTime? createdDate = null, bool isUsed = false)
        {
            var date = createdDate ?? DateTime.Now;
            var data = new FileManagement
            {
                Name = fileName,
                Extension = fileExtension,
                Type = (int)type,
                CreatedDate = date.ToTimeStamp(),
                IsUsed = isUsed
            };

            await _repository.AddAsync(data);

            return data;
        }

        public async Task<FileManagement> AddImages(string fileName, string fileExtension)
        {
            return await Add(fileName, fileExtension, FileType.PostImage);
        }
        
        public async Task<FileManagement> AddThumbnail(string fileName, string fileExtension)
        {
            return await Add(fileName, fileExtension, FileType.PostThumbnail);
        }

        public async Task<List<FileManagement>> GetUnusedFilesAsync()
        {
            var lastDay = DateTime.Now.AddDays(-1).ToTimeStamp();
            return await _repository.GetAll(_ => _.CreatedDate < lastDay && !_.IsUsed)
                .AsNoTracking().ToListAsync();
        }
    }
}

using Microsoft.AspNetCore.Hosting.Server;
using NEWS.Entities.Constants;
using NEWS.Entities.Exceptions;
using NEWS.Entities.Models.Others;
using NEWS.Entities.MySqlEntities;
using NEWS.Entities.Services;
using NEWS.Entities.Utils;
using System.Text.RegularExpressions;

namespace NEWS.WebAPI.Services
{
    public interface IFileService
    {
        Task<FileInformation> UploadAsync(IFormFile formFile, FileType type, CancellationToken cancellationToken = default);
        Task<FileInformation> UploadImageAsync(IFormFile formFile, CancellationToken cancellationToken = default);
        Task<FileInformation> UploadBase64Async(ImageInfo info, FileType type);

        Task<FileInformation> UploadThumbnailBase64Async(ImageInfo info);

        Task DeletedUnusedFilesAsync();
    }

    public class FileService : IFileService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger _logger;

        public FileService(IServiceProvider serviceProvider, ILogger<FileService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public async Task<FileInformation> UploadAsync(IFormFile file, FileType type, CancellationToken cancellationToken = default)
        {
            // Check if the file exists else, throw an exception        
            if (file.Length < 1) throw new Exception($"No file found!");

            // State the file extensions you require to be uploaded.
            var allowedFileTypes = new[] { "png", "jpg", "jpeg", "svg", "bmp", "webp" };

            // Get the file extension.
            var fileExtension = Path.GetExtension(file.FileName).Substring(1).ToLower();

            // validate the file extension type.
            if (!allowedFileTypes.Contains(fileExtension))
            {
                throw new BusinessException($"Chỉ chấp nhận ảnh có đuôi {String.Join(", ", allowedFileTypes)} - Đuôi ảnh hiện tại: {Path.GetExtension(file.FileName)} ");
            }

            var fileName = GenerateFileName(fileExtension, type);

            // Create an instance of the memory stream.
            await using var memoryStream = new MemoryStream();

            // Write file to Stream
            await file.CopyToAsync(memoryStream, cancellationToken);

            // Read from the start of the memoryStream
            memoryStream.Position = 0;

            // Write file to System path
            var filePath = Path.Combine($"{FileUtils.GetFolderPath(type)}/", fileName);
            using var fileStream = new FileStream(filePath, FileMode.Create);

            // Write Memory Stream to FileStream
            await memoryStream.CopyToAsync(fileStream, cancellationToken);

            return new FileInformation
            {
                Name = fileName,
                Extension= fileExtension,
            };
        }

        public async Task<FileInformation> UploadBase64Async(ImageInfo info, FileType type)
        {
            if (info == null || string.IsNullOrEmpty(info.Base64))
            {
                return null;
            }

            var fileExtension = Path.GetExtension(info.Name).Substring(1).ToLower();
            var fileName = GenerateFileName(fileExtension, type);

            var bytes = Convert.FromBase64String(Regex.Replace(info.Base64, @"data:image/.*base64,", ""));
            var filePath = Path.Combine($"{FileUtils.GetFolderPath(type)}/", fileName);
            await File.WriteAllBytesAsync(filePath, bytes);

            return new FileInformation
            {
                Name = fileName,
                Extension = fileExtension,
            };
        }

        public async Task<FileInformation> UploadThumbnailBase64Async(ImageInfo info)
        {
            return await UploadBase64Async(info, FileType.PostThumbnail);
        }

        public async Task<FileInformation> UploadImageAsync(IFormFile formFile, CancellationToken cancellationToken = default)
        {
            return await UploadAsync(formFile, FileType.PostImage, cancellationToken);
        }

        private string GenerateFileName(string fileExtension, FileType type)
        {
            var fileName = $"{Guid.NewGuid()}.{fileExtension}";

            switch (type)
            {
                case FileType.PostImage:
                    return $"{AppConst.IMAGE_POST_PREFIX}{fileName}";
                default:
                    return fileName;
            }
        }

        public async Task DeletedUnusedFilesAsync()
        {
            var now = DateTime.UtcNow;
            var fileManageService = _serviceProvider.CreateScope().ServiceProvider.GetRequiredService<IFileManagementService>();
            var files = await fileManageService.GetUnusedFilesAsync();
            var errorCount = 0;
            foreach (var file in files)
            {
                try
                {
                    DeleteFileFromDisk(file);
                    await fileManageService.DeleteAsync(file);
                }
                catch (Exception e)
                {
                    errorCount++;
                    _logger.LogError(e, $"Error when delete unused file - FileId: {file.Id} - {e.Message} - {e.StackTrace}");
                }
            }

            var time = DateTime.UtcNow - now;
            _logger.LogInformation($"Deleted unused file successfully - Time: {time.TotalMilliseconds}ms - Total: {files.Count} - Errors: {errorCount}");
        }

        private void DeleteFileFromDisk(FileManagement file)
        {
            string filePath = Path.Combine($"{FileUtils.GetFolderPath(file.Type)}/", file.Name);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NEWS.Entities.Services;
using NEWS.WebAPI.Services;

namespace NEWS.WebAPI.Controllers.Admin
{
    [Route("api/admin/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin, Editor")]
    public class FileController : ControllerBase
    {
        private IFileService _fileService;
        private IFileManagementService _fileManagementService;

        public FileController(IFileService fileService,
            IFileManagementService fileManagementService)
        {
            _fileService = fileService;
            _fileManagementService = fileManagementService;
        }

        [HttpPost("image")]
        public async Task<ActionResult> UploadImageAsync(IFormFile image)
        {
            var fileInfo = await _fileService.UploadImageAsync(image);
            var result = await _fileManagementService.AddImages(fileInfo.Name, fileInfo.Extension);

            return Ok(result);
        }
    }
}

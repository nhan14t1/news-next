using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NEWS.Entities.Models.ViewModels;
using NEWS.Entities.Services;
using NEWS.WebAPI.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NEWS.WebAPI.Controllers.Admin
{
    [Route("api/admin/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;
        private readonly IFileService _fileService;
        private readonly IFileManagementService _fileManagementService;

        public PostController(IPostService postService,
            IFileService fileService,
            IFileManagementService fileManagementService)
        {
            _postService = postService;
            _fileService = fileService;
            _fileManagementService = fileManagementService;
        }

        [HttpPost]
        public async Task<ActionResult> Add([FromBody] PostVM request)
        {
            var email = User.Identity.Name;

            var fileInfo = await _fileService.UploadThumbnailBase64Async(request.Thumbnail);
            var fileThumbnail = fileInfo != null ? await _fileManagementService.AddThumbnail(fileInfo.Name, fileInfo.Extension) : null;
            var post = await _postService.AddAsync(request, email, fileThumbnail);
            return Ok(post);
        }

        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            var data = await _postService.GetAllAsync();
            return Ok(data);
        }

        // GET api/<PostController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // PUT api/<PostController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<PostController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NEWS.Entities.Services;

namespace NEWS.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;
        private ILogger<PostController> _logger;
        public PostController(IPostService postService)
        {
            _postService = postService;
        }

        [HttpGet("home-page")]
        public async Task<ActionResult> GetHomePageData()
        {
            _logger.LogInformation("Access home page test");
            var data = await _postService.GetHomePageData();
            return Ok(data);
        }

        [HttpGet("{slug}")]
        public async Task<ActionResult> GetBySlug(string slug)
        {
            var data = await _postService.GetBySlugAsync(slug);
            return Ok(data);
        }

        [HttpGet("site-map")]
        public async Task<ActionResult> GetPostMap()
        {
            _logger.LogInformation("Access site map");

            try
            {
                var data = await _postService.GetPostMap();
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"GetPostMap Exception {ex.Message} - {ex.StackTrace}");
                throw;
            }
        }
    }
}

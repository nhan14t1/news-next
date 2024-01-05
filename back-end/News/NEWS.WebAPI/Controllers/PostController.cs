using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NEWS.Entities.Constants;
using NEWS.Entities.Models.ViewModels;
using NEWS.Entities.Services;
using NEWS.Entities.Utils;
using NEWS.Services.Services;

namespace NEWS.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;
        private ILogger<PostController> _logger;
        public PostController(IPostService postService, ILogger<PostController> logger)
        {
            _postService = postService;
            _logger = logger;
        }

        [HttpGet("home-page")]
        public async Task<ActionResult> GetHomePageData()
        {
            var data = await _postService.GetHomePageData();
            return Ok(data);
        }

        [HttpGet("{slug}")]
        public async Task<ActionResult> GetBySlug(string slug)
        {
            var data = await _postService.GetBySlugAsync(slug);
            return Ok(data);
        }

        [HttpGet("preview/{slug}")]
        public async Task<ActionResult> GetPreviewBySlug(string slug)
        {
            var data = await _postService.GetBySlugAsync(slug, true);
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
                _logger.LogError(ex, $"{ex.Message} - {ex.StackTrace}");
                throw;
            }
        }

        [HttpPut("views/{postId}")]
        public async Task<ActionResult> UpdateViews(int postId)
        {
            var isAdmin = RoleUtils.ADMIN_EDITOR_ROLE_NAMES.Any(role => User.IsInRole(role));

            // Don't flag when admin view post
            if (!isAdmin)
            {
                await _postService.UpdateViews(postId);
            }
            return Ok(true);
        }
    }
}

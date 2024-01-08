using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NEWS.Entities.Models.ViewModels;
using NEWS.Entities.Services;

namespace NEWS.WebAPI.Controllers.Admin
{
    [Route("api/admin/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin, Editor")]
    public class TagController : ControllerBase
    {
        private readonly ITagService _tagService;

        public TagController(ITagService tagService)
        {
            _tagService = tagService;
        }

        [HttpPost("search")]
        public async Task<ActionResult> SearchAsync([FromBody] SearchTagVM request)
        {
            var data = await _tagService.SearchAsync(request.Keyword);
            return Ok(data);
        }
    }
}

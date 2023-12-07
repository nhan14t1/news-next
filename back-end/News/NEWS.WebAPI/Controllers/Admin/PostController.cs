using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NEWS.Entities.MySqlEntities;
using NEWS.Entities.Responses;
using NEWS.Entities.Services;
using NEWS.Entities.ViewModels;
using NEWS.Services.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NEWS.WebAPI.Controllers.Admin
{
    [Route("api/admin/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;
        private readonly IUserService _userService;
        public PostController(IPostService postService,
            IUserService userService)
        {
            _postService = postService;
            _userService = userService;
        }

        [HttpPost]
        public async Task<ActionResult> Add([FromBody] PostVM request)
        {
            var email = User.Identity.Name;
            var test = User.Claims.ToList();
            var post = await _postService.AddAsync(request, email);
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

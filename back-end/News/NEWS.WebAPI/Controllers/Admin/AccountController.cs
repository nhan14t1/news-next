using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NEWS.Entities.Models.ViewModels;
using NEWS.Entities.Services;
using NEWS.Services.Services;
using NEWS.WebAPI.Services;

namespace NEWS.WebAPI.Controllers.Admin
{
    [Route("api/admin/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AccountController : ControllerBase
    {
        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: api/<AccountController>
        [HttpGet]
        public async Task<ActionResult> GetListAsync()
        {
            var data = await _userService.GetAllAsync();
            return Ok(data);
        }

        // GET api/<AccountController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        [HttpPost]
        public async Task<ActionResult> AddAsync([FromBody] UserVM request)
        {
            
            var user = await _userService.CreateUserAsync(request);
            return Ok(user);
        }

        // PUT api/<AccountController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<AccountController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

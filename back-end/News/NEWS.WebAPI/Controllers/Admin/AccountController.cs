﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NEWS.Entities.Models.ViewModels;
using NEWS.Entities.Services;

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

        [HttpPut]
        public async Task<ActionResult> UpdateAsync([FromBody] UserVM request)
        {
            var user = await _userService.UpdateUserAsync(request);
            return Ok(user);
        }

        // DELETE api/<AccountController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

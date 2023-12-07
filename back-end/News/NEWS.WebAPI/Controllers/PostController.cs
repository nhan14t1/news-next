﻿using Microsoft.AspNetCore.Authorization;
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
        public PostController(IPostService postService)
        {
            _postService = postService;
        }

        [HttpGet("home-page")]
        public async Task<ActionResult> GetHomePageData()
        {
            var data = await _postService.GetHomePageData();
            return Ok(data);
        }
    }
}

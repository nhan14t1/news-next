using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NEWS.Entities.Models.Responses;
using NEWS.Entities.Models.ViewModels;
using NEWS.Entities.MySqlEntities;
using NEWS.Entities.Services;
using NEWS.WebAPI.JwtUtils;

namespace NEWS.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IJwtAuthManager _jwtAuthManager;
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;

        public AccountController(IJwtAuthManager jwtAuthManager, IUserService userService,
            IRoleService roleService)
        {
            _jwtAuthManager = jwtAuthManager;
            _userService = userService;
            _roleService = roleService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginVM request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (!await _userService.ValidateUserAsync(request.UserName, request.Password))
            {
                return Unauthorized();
            }

            var user = await _userService.GetByEmailAsync(request.UserName);
            var roles = _roleService.GetByEmail(request.UserName);
            var claims = _jwtAuthManager.GetClaims(request.UserName, roles);
            var jwtResult = _jwtAuthManager.GenerateTokens(request.UserName, claims, DateTime.UtcNow);

            return Ok(new LoginResult
            {
                Id = user.Id,
                UserName = request.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Roles = roles.Select(_ => new Role
                {
                    Id = _.Id,
                    Name = _.Name
                }).ToList(),
                AccessToken = jwtResult.AccessToken,
                TokenExpirationInTimeStamp = jwtResult.AccessTokenExprationTimestamp
            }) ;
        }

        [AllowAnonymous]
        [HttpGet("init-user")]
        public string InitUser()
        {
            _userService.CreateUserAsync("nhan.14t1@gmail.com", "123456", "Nhan", "Phan",
                27, "0983592287", false).Wait();
            _userService.CreateUserAsync("admin", "vStation123", "Nhan", "Phan",
                27, "0983592287", true).Wait();
            return "OK";
        }

        [AllowAnonymous]
        [HttpGet("test")]
        public string InitUser()
        {
            return "OK";
        }

        [HttpPost("logout")]
        [Authorize]
        public ActionResult Logout()
        {
            // optionally "revoke" JWT token on the server side --> add the current token to a block-list
            // https://github.com/auth0/node-jsonwebtoken/issues/375

            var userName = User.Identity?.Name;
            return Ok();
        }

        //[HttpPost("refresh-token")]
        //[Authorize]
        //public async Task<ActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
        //{
        //    try
        //    {
        //        var userName = User.Identity?.Name;
        //        _logger.LogInformation($"User [{userName}] is trying to refresh JWT token.");

        //        if (string.IsNullOrWhiteSpace(request.RefreshToken))
        //        {
        //            return Unauthorized();
        //        }

        //        var accessToken = await HttpContext.GetTokenAsync("Bearer", "access_token");
        //        var jwtResult = _jwtAuthManager.Refresh(request.RefreshToken, accessToken, DateTime.Now);
        //        _logger.LogInformation($"User [{userName}] has refreshed JWT token.");
        //        return Ok(new LoginResult
        //        {
        //            UserName = userName,
        //            Role = User.FindFirst(ClaimTypes.Role)?.Value ?? string.Empty,
        //            AccessToken = jwtResult.AccessToken,
        //            RefreshToken = jwtResult.RefreshToken.TokenString
        //        });
        //    }
        //    catch (SecurityTokenException e)
        //    {
        //        return Unauthorized(e.Message); // return 401 so that the client side can redirect the user to login page
        //    }
        //}

        //[HttpPost("impersonation")]
        //[Authorize(Roles = UserRoles.Admin)]
        //public ActionResult Impersonate([FromBody] ImpersonationRequest request)
        //{
        //    var userName = User.Identity?.Name;
        //    _logger.LogInformation($"User [{userName}] is trying to impersonate [{request.UserName}].");

        //    var impersonatedRole = _userService.GetUserRole(request.UserName);
        //    if (string.IsNullOrWhiteSpace(impersonatedRole))
        //    {
        //        _logger.LogInformation($"User [{userName}] failed to impersonate [{request.UserName}] due to the target user not found.");
        //        return BadRequest($"The target user [{request.UserName}] is not found.");
        //    }
        //    if (impersonatedRole == UserRoles.Admin)
        //    {
        //        _logger.LogInformation($"User [{userName}] is not allowed to impersonate another Admin.");
        //        return BadRequest("This action is not supported.");
        //    }

        //    var claims = new[]
        //    {
        //        new Claim(ClaimTypes.Name,request.UserName),
        //        new Claim(ClaimTypes.Role, impersonatedRole),
        //        new Claim("OriginalUserName", userName ?? string.Empty)
        //    };

        //    var jwtResult = _jwtAuthManager.GenerateTokens(request.UserName, claims, DateTime.Now);
        //    _logger.LogInformation($"User [{request.UserName}] is impersonating [{request.UserName}] in the system.");
        //    return Ok(new LoginResult
        //    {
        //        UserName = request.UserName,
        //        Role = impersonatedRole,
        //        OriginalUserName = userName,
        //        AccessToken = jwtResult.AccessToken,
        //        RefreshToken = jwtResult.RefreshToken.TokenString
        //    });
        //}

        //[HttpPost("stop-impersonation")]
        //public ActionResult StopImpersonation()
        //{
        //    var userName = User.Identity?.Name;
        //    var originalUserName = User.FindFirst("OriginalUserName")?.Value;
        //    if (string.IsNullOrWhiteSpace(originalUserName))
        //    {
        //        return BadRequest("You are not impersonating anyone.");
        //    }
        //    _logger.LogInformation($"User [{originalUserName}] is trying to stop impersonate [{userName}].");

        //    var role = _userService.GetUserRole(originalUserName);
        //    var claims = new[]
        //    {
        //        new Claim(ClaimTypes.Name,originalUserName),
        //        new Claim(ClaimTypes.Role, role)
        //    };

        //    var jwtResult = _jwtAuthManager.GenerateTokens(originalUserName, claims, DateTime.Now);
        //    _logger.LogInformation($"User [{originalUserName}] has stopped impersonation.");
        //    return Ok(new LoginResult
        //    {
        //        UserName = originalUserName,
        //        Role = role,
        //        OriginalUserName = null,
        //        AccessToken = jwtResult.AccessToken,
        //        RefreshToken = jwtResult.RefreshToken.TokenString
        //    });
        //}
    }
}

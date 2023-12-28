using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using NEWS.Entities.Constants;

namespace NEWS.Entities.Extensions
{
    public static class HttpContextExtensions
    {
        public static int GetUserId(this HttpContext httpContext)
        {
            var userIdClaim = httpContext.User.Claims.FirstOrDefault(_ => _.Type == ClaimConst.USER_ID);
            var userId = int.Parse(userIdClaim?.Value ?? "0");

            return userId;
        }

        public static string GetAccessToken(this HttpContext httpContext)
        {
            return httpContext.Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
        }
    }
}

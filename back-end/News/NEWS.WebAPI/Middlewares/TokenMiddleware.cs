using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using NEWS.Entities.Constants;
using NEWS.Entities.Exceptions;
using NEWS.Entities.Extensions;
using NEWS.Entities.Models.Responses;
using NEWS.Entities.Services;
using System.Net;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace NEWS.WebAPI.Middlewares
{
    public class TokenMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public TokenMiddleware(RequestDelegate next,
            ILogger<ExceptionMiddleware> logger
            )
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext, IUserTokenService userTokenService)
        {
            var userId = httpContext.GetUserId();
            var token = httpContext.GetAccessToken();
            if (userTokenService.IsTokenBlocked(userId, token))
            {
                await FlagUnauthorized(httpContext);
            }
            else
            {
                await _next(httpContext);
            }
        }

        private async Task FlagUnauthorized(HttpContext context)
        {
            context.Response.Headers.Add("Access-Control-Allow-Origin", "*");
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = 401;
            var error = new ErrorResult(401, "Phiên đăng nhập hết hạn");
            await context.Response.WriteAsync
            (
                JsonSerializer.Serialize(error, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase })
            );
        }
    }
}

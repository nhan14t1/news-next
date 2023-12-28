using NEWS.Entities.Constants;
using NEWS.Entities.Exceptions;
using NEWS.Entities.Models.Responses;
using NEWS.Entities.Utils;
using System.Buffers.Text;
using System.Net;
using System.Text.Json;

namespace NEWS.WebAPI.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            if (context.Response.StatusCode == (int)HttpStatusCode.Unauthorized)
            {
                // Handle unauth
            }

            var baseEx = ex.GetBaseException();
            //
            // Return as json
            context.Response.ContentType = "application/json";

            // Convert to model
            var error = new ErrorResult(baseEx.Message);
            if (ex is BusinessException)
            {
                context.Response.StatusCode = 700;
                error.StatusCode = 700;
                _logger.LogInformation(baseEx, $"{baseEx.Message} - {baseEx.StackTrace}");
            }
            else if (ex is UnauthorizedException)
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                error.StatusCode = (int)HttpStatusCode.Unauthorized;
            }
            else
            {
                context.Response.StatusCode = 500;
                error.StatusCode = 500;
                error.Message = "Sorry, an error has occurred";
                _logger.LogError(baseEx, $"{baseEx.Message} - {baseEx.StackTrace}");
            }

            await context.Response.WriteAsync
            (
                JsonSerializer.Serialize(error, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase })
            );
        }
    }
}

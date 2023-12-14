using NEWS.Entities.Constants;
using NEWS.Entities.Exceptions;
using NEWS.Entities.Models.Responses;
using NEWS.Entities.Utils;
using System.Net;
using System.Text.Json;

namespace NEWS.WebAPI.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        //private readonly ILogger<ExceptionMiddleware> _logger;
        private static ILoggerFactory loggerFactory = new LoggerFactory();

        private static ILogger _logger = loggerFactory.CreateLogger(nameof(ExceptionMiddleware));

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
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
            //var error = new ErrorResult("Sorry, an error has occurred");
            var error = new ErrorResult($"{baseEx.Message} - {baseEx.StackTrace}");
            if (ex is BusinessException)
            {
                context.Response.StatusCode = 700;
                error.StatusCode = 700;
                error.Message = baseEx.Message;
                _logger.LogInformation(baseEx, baseEx.Message);
            }
            else
            {
                _logger.LogError(baseEx, baseEx.Message);
            }

            await context.Response.WriteAsync
            (
                JsonSerializer.Serialize(error, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase })
            );
        }
    }
}

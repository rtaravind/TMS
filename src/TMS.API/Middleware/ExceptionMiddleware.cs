using System.Net;
using System.Text.Json;
using TMS.API.Exceptions;
using ValidationException = TMS.API.Exceptions.ValidationException;

namespace TMS.API.Middleware
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

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong: {ex.Message}");
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var statusCode = HttpStatusCode.InternalServerError;

            var result = new
            {
                message = "Internal Server Error",
                detail = exception.Message
            };

            if (exception is NotFoundException)
            {
                statusCode = HttpStatusCode.NotFound;
                result = new { message = "Resource not found", detail = exception.Message };
            }
            else if (exception is UnauthorizedAccessException)
            {
                statusCode = HttpStatusCode.Unauthorized;
                result = new { message = "Unauthorized", detail = exception.Message };
            }
            else if (exception is ValidationException)
            {
                statusCode = HttpStatusCode.BadRequest;
                result = new { message = "Validation error", detail = exception.Message };
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            var response = JsonSerializer.Serialize(result);
            return context.Response.WriteAsync(response);
        }
    }
}

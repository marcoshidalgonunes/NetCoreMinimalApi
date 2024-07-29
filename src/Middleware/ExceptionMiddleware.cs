using System.Net;
using System.Text;
using System.Text.Json;

namespace NetCoreMinimalApi.Middleware;

public class ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
{
    private readonly RequestDelegate _next = next;
    private readonly ILogger<ExceptionMiddleware> _logger = logger;

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception has occurred.");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var isValidationException = exception is FluentValidation.ValidationException;
        var statusCode = isValidationException
            ? (int)HttpStatusCode.UnprocessableEntity
            : (int)HttpStatusCode.InternalServerError;

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;

        var response = isValidationException
            ? new
            {
                statusCode,
                Message = "One or more fields are invalid",
                Detailed = ((FluentValidation.ValidationException)exception).Errors
                    .GroupBy(error => error.PropertyName)
                    .Select(group => group.First().PropertyName)
                    .Aggregate(new StringBuilder(), (sb, propertyName) =>
                    {
                        sb.AppendLine(propertyName);
                        return sb;
                    })
                    .ToString()
            }
            : new
            {
                statusCode,
                Message = "Internal Server Error. Please try again later.",
                Detailed = exception.Message
            };

        var jsonResponse = JsonSerializer.Serialize(response);
        return context.Response.WriteAsync(jsonResponse);
    }
}

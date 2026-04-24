using Backend.Core.DTOs;
using Backend.Core.Exceptions;

namespace Backend.API.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<ExceptionHandlingMiddleware> logger)
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
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        var traceId = context.TraceIdentifier;

        var (statusCode, message, errors) = ex switch
        {
            NotFoundException nfe =>
                (StatusCodes.Status404NotFound, nfe.Message, null),

            ValidationException ve =>
                (StatusCodes.Status400BadRequest, ve.Message, ve.Errors),

            ArgumentOutOfRangeException aore =>
                (StatusCodes.Status400BadRequest, aore.Message, null),

            _ => (StatusCodes.Status500InternalServerError,
                "An unexpected error occurred.", null)
        };

        if (statusCode == StatusCodes.Status500InternalServerError)
            _logger.LogError(ex, "Unhandled exception. TraceId: {TraceId}", traceId);
        else
            _logger.LogWarning(ex, "Handled exception. TraceId: {TraceId}", traceId);

        var response = new ErrorResponse
        {
            StatusCode = statusCode,
            Message    = message,
            Errors     = errors,
            TraceId    = traceId
        };

        context.Response.StatusCode      = statusCode;
        context.Response.ContentType     = "application/json";

        await context.Response.WriteAsJsonAsync(response);
    }
    
}
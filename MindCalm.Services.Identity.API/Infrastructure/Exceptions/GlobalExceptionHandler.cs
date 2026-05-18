using System.Diagnostics;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MindCalm.Services.Identity.API.Infrastructure.Exceptions;

public class GlobalExceptionHandler(
    ILogger<GlobalExceptionHandler> logger,
    IProblemDetailsService problemDetailsService) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception,
        CancellationToken cancellationToken)
    {
        var traceId = Activity.Current?.Id ?? httpContext.TraceIdentifier;

        // Determine the status code and safe message based on the exception type
        var (statusCode, title, message) = exception switch
        {
            DbUpdateException => (StatusCodes.Status500InternalServerError,
                "Database error",
                "A critical database error occurred. Please try again later."),

            UnauthorizedAccessException => (StatusCodes.Status401Unauthorized,
                "Unauthorized",
                "You are not authorized to perform this action."),

            _ => (StatusCodes.Status500InternalServerError, "Internal error", "An unexpected error occurred.")
        };
        
        var level = statusCode >= 500 ? LogLevel.Error : LogLevel.Warning;
        logger.Log(level, exception, "Handled exception: [{TraceId}] - {ErrorMessage}", traceId, message);

        if (httpContext.Response.HasStarted) return false;
        
        // Set up the HTTP response
        httpContext.Response.StatusCode = statusCode;

        // Wrap the error in your standard Result object for the Angular frontend
        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Detail = message,
            Type = $"https://httpstatuses.com/{statusCode}",
            Instance = httpContext.Request.Path,
            Extensions =
            {
                ["traceId"] = traceId
            }
        };

        await problemDetailsService.WriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            ProblemDetails = problemDetails
        });

        // Return true to tell ASP.NET Core "I handled this exception, stop processing."
        return true;
    }
}
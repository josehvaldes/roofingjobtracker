using JobTracker.Application.Common.Exceptions;
using JobTracker.Domain.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace JobTracker.API.Middleware
{
    public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
    {

        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            var (statusCode, title) = exception switch
            {
                EntityNotFoundException => (StatusCodes.Status404NotFound, "Not Found"),
                ValidationException => (StatusCodes.Status422UnprocessableEntity, "Validation Error"),
                InvalidEntityException => (StatusCodes.Status400BadRequest, "Invalid Entity"),
                InvalidJobTransitionException => (StatusCodes.Status400BadRequest, "Invalid Job Transition"),

                _ => (StatusCodes.Status500InternalServerError, "Server Error")
            };

            logger.LogError(
                exception,
                "Exception: {Type} | Status: {StatusCode} | Message: {Message}",
                exception.GetType().Name,
                statusCode,
                exception.Message);

            ProblemDetails problemDetails = exception switch
            {
                ValidationException validationException => new ValidationProblemDetails(validationException.Errors)
                {
                    Status = statusCode,
                    Title = title,
                    Detail = exception.Message,
                    Instance = httpContext.Request.Path
                },
                _ => new ProblemDetails
                {
                    Status = statusCode,
                    Title = title,
                    Detail = exception.Message,
                    Instance = httpContext.Request.Path
                }
            };

            httpContext.Response.StatusCode = statusCode;

            await httpContext.Response.WriteAsJsonAsync(problemDetails, problemDetails.GetType(), cancellationToken);

            return true; // true = exception is handled, do not propagate further
        }
    }
}

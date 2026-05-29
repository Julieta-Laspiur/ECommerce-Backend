using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ECommerce.Domain.Exceptions;
public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;
    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    => _logger = logger;
    public async ValueTask<bool> TryHandleAsync(
    HttpContext httpContext,
    Exception exception,
    CancellationToken cancellationToken)
    {
        // 1. Siempre loguear con stacktrace completo antes de responder
        _logger.LogError(exception,
        "Unhandled exception: {Message}", exception.Message);
        // 2. Mapear tipo de excepción → HTTP status + título
        int statusCode = StatusCodes.Status500InternalServerError;

        string title = "An unexpected error occurred";

        switch (exception)
        {
            case ValidationException:
                statusCode = StatusCodes.Status400BadRequest;
                title = "Validation failed";
                break;

            case NotFoundException:
                statusCode = StatusCodes.Status404NotFound;
                title = exception.Message;
                break;

            case DomainException:
                statusCode = StatusCodes.Status422UnprocessableEntity;
                title = exception.Message;
                break;
        }
        // 3. Escribir la respuesta en formato ProblemDetails
        httpContext.Response.StatusCode = statusCode;
        await httpContext.Response.WriteAsJsonAsync(
        new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Instance = httpContext.Request.Path
        },
        cancellationToken);
        // 4. Retornar true indica que la excepción fue manejada
        return true;
    }
}
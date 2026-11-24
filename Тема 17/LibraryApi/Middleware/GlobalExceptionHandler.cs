using LibraryApi.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApi.Middleware;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        => _logger = logger;

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext, Exception exception, CancellationToken ct)
    {
        var statusCode = exception is AppException appEx
            ? appEx.StatusCode
            : StatusCodes.Status500InternalServerError;

        if (exception is AppException)
            _logger.LogWarning("Ожидаемая ошибка запроса: {Message}", exception.Message);
        else
            _logger.LogError(exception, "Необработанное исключение");

        httpContext.Response.StatusCode = statusCode;

        await httpContext.Response.WriteAsJsonAsync(new ProblemDetails
        {
            Status = statusCode,
            Title = statusCode == 500
                ? "Внутренняя ошибка сервера"
                : exception.Message,
        }, ct);

        return true;
    }
}
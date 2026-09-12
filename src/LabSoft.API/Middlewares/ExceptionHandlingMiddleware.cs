using System.Net;
using System.Text.Json;

namespace LabSoft.API.Middlewares;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            // Deja que la petición siga su camino hacia el controlador
            await _next(context);
        }
        catch (Exception ex)
        {
            // Si ocurre CUALQUIER error no controlado, lo atrapamos aquí
            _logger.LogError(ex, "Ocurrió un error no controlado: {Message}", ex.Message);

            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        // Determinamos el código de estado HTTP según el tipo de excepción
        var statusCode = exception switch
        {
            UnauthorizedAccessException => (int)HttpStatusCode.Unauthorized,
            KeyNotFoundException => (int)HttpStatusCode.NotFound,
            InvalidOperationException => (int)HttpStatusCode.BadRequest,
            _ => (int)HttpStatusCode.InternalServerError // Por defecto 500 para errores desconocidos
        };

        context.Response.StatusCode = statusCode;

        // Creamos una respuesta JSON estandarizada y limpia
        var response = new
        {
            success = false,
            status = statusCode,
            message = statusCode == 500 
                ? "Ocurrió un error interno en el servidor. Por favor, intente más tarde." 
                : exception.Message // Si es una regla de negocio controlada, mostramos el mensaje exacto
        };

        var jsonResponse = JsonSerializer.Serialize(response);
        return context.Response.WriteAsync(jsonResponse);
    }
}
using System.Net;
using System.Text.Json;

namespace projServer.Middlewares
{
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
            _logger.LogWarning("Exception middleware active for {Path}", context.Request.Path);

            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[{TraceId}] Unhandled exception", context.TraceIdentifier);

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var isDev = context.RequestServices
                    .GetRequiredService<IWebHostEnvironment>().IsDevelopment();
                string errorCode = ex.GetType().Name switch
                {
                    nameof(ArgumentNullException) => "ERR_NULL_ARGUMENT",
                    nameof(ArgumentException) => "ERR_INVALID_ARGUMENT",
                    nameof(InvalidOperationException) => "ERR_INVALID_OPERATION",
                    nameof(UnauthorizedAccessException) => "ERR_UNAUTHORIZED",
                    _ => "ERR_UNKNOWN"
                };

                var response = new
                {
                    statusCode = context.Response.StatusCode,
                    message = "An unexpected error occurred.",
                    detailed = isDev ? ex.Message : null,
                    errorType = ex.GetType().Name,
                    errorCode,
                    traceId = context.TraceIdentifier,
                    timestamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ")
                };

                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
            }
        }
    }
}

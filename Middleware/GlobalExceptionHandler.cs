using System.Net;
using System.Text.Json;
using Ctrl_Save.Models;
using Microsoft.EntityFrameworkCore;

namespace Ctrl_Save.Middleware
{
    public class GlobalExceptionHandler
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandler> _logger;
        private readonly IServiceProvider _serviceProvider;

        public GlobalExceptionHandler(RequestDelegate next, ILogger<GlobalExceptionHandler> logger, IServiceProvider serviceProvider)
        {
            _next = next;
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception: {Message}", ex.Message);
                await LogToDatabase(context, ex);
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task LogToDatabase(HttpContext context, Exception ex)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<Ctrl_SaveContext>();

                var errorLog = new ErrorLog
                {
                    Message = ex.Message,
                    StackTrace = ex.StackTrace,
                    Path = context.Request.Path,
                    Method = context.Request.Method,
                    StatusCode = 500,
                    OccurredAt = DateTime.UtcNow
                };

                dbContext.ErrorLogs.Add(errorLog);
                await dbContext.SaveChangesAsync();
            }
            catch (Exception dbEx)
            {
                _logger.LogError(dbEx, "Failed to log error to database.");
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var response = new
            {
                status = 500,
                error = "An unexpected error occurred.",
                message = ex.Message,
                timestamp = DateTime.UtcNow
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}

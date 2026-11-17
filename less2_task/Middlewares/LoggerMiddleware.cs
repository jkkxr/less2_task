using less2_task.Services;

namespace less2_task.Middlewares
{
    public class LoggerMiddleware
    {
        private readonly RequestDelegate _next;
        public LoggerMiddleware(RequestDelegate next)
        {
            _next = next;
    
        }
        public async Task InvokeAsync(HttpContext context, RequestLoggerService requestLoggerService)
        {
            await requestLoggerService.LogRequestAsync();
            await _next(context);
        }
    }
}

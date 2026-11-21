namespace less2_task.Services
{
    public class RequestLoggerService
    {
        private static readonly SemaphoreSlim _fileLock = new(1, 1);
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RequestLoggerService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task LogRequestAsync()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null)
            {
                return;
            }

            var logMessage = $"{DateTime.UtcNow}: {httpContext.Request.Method} {httpContext.Request.Path} IP:{httpContext.Connection.RemoteIpAddress}";
            Console.WriteLine(logMessage);

            await _fileLock.WaitAsync();
            try
            {
                await File.AppendAllTextAsync("requests.log", logMessage + Environment.NewLine);
            }
            finally
            {
                _fileLock.Release();
            }
        }
    }
}

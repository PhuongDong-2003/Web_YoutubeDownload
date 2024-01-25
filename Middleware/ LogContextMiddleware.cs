using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Serilog.Context;

namespace YoutubeDownload.Middleware
{
    public class LogContextMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public LogContextMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            _next = next;
            _logger = loggerFactory.CreateLogger<LogContextMiddleware>();
        }

        public async Task Invoke(HttpContext context, IHttpContextAccessor httpContextAccessor)
        {
            using (LogContext.PushProperty("Name", httpContextAccessor.HttpContext?.User?.Identity?.Name))
            {
                
                await _next(context);

            }
        }
    }
}
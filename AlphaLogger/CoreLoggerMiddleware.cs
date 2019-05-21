using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace AlphaCoreLogger
{

    public class CoreLoggerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ICoreLogger _logger;

        public CoreLoggerMiddleware(RequestDelegate next, ICoreLogger logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {

                await _logger.LogAsync(context,ex);
                context.Response.Redirect("/Error");
            }
        }
    }
}

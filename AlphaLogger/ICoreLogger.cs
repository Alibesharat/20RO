using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AlphaCoreLogger
{
    public interface ICoreLogger
    {
        void Log(string Message);
        void Log(Exception exception);
        void Log(CoreLog log);

        void Log(HttpContext httpContext, string Message);
        void Log(HttpContext httpContext, Exception exception);

        //---- async -------

        Task LogAsync(string Message);
        Task LogAsync(Exception exception);

        Task LogAsync(HttpContext httpContext, Exception exception);
        Task LogAsync(HttpContext httpContext, string Message);
        Task LogAsync(CoreLog log);


        List<CoreLog> ReadLogs();
        Task<List<CoreLog>> ReadLogsAsync();


        void ClearLogs();
    }
}

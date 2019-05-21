using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace AlphaCoreLogger
{
    public class DiskLogger : ICoreLogger
    {


        public void Log(string Message)
        {
            var log = new CoreLog()
            {
                DateTime = DateTime.Now,
                ErrorMessage = Message,
                Exception = null,
                RequsetPath = "?",
                UserAgent = "?",
                UserIp = "?"

            };
            WriteFile(log);
        }

        public void Log(Exception exception)
        {
            var log = new CoreLog()
            {
                DateTime = DateTime.Now,
                ErrorMessage = exception.Message,
                Exception = exception,
                RequsetPath = "?",
                UserAgent = "?",
                UserIp = "?"

            };
            WriteFile(log);
        }

        public void Log(HttpContext httpContext, string Message)
        {

            var log = new CoreLog()
            {
                DateTime = DateTime.Now,
                ErrorMessage = Message,
                Exception = null,
                RequsetPath = httpContext.Request.Path,
                UserAgent = "?",
                UserIp = "?"

            };
            WriteFile(log);
        }

        public void Log(HttpContext httpContext, Exception exception)
        {
            var log = new CoreLog()
            {
                DateTime = DateTime.Now,
                ErrorMessage = exception.Message,
                Exception = exception,
                RequsetPath = httpContext.Request.Path,
                UserAgent = "?",
                UserIp = "?"

            };
            WriteFile(log);
        }

        public async Task LogAsync(string Message)
        {
            var log = new CoreLog()
            {
                DateTime = DateTime.Now,
                ErrorMessage = Message,
                Exception = null,
                RequsetPath = "?",
                UserAgent = "?",
                UserIp = "?"

            };
            await WriteFileAsync(log);
        }

        public async Task LogAsync(Exception exception)
        {
            var log = new CoreLog()
            {
                DateTime = DateTime.Now,
                ErrorMessage = exception.Message,
                Exception = null,
                RequsetPath = "?",
                UserAgent = "?",
                UserIp = "?"

            };
            await WriteFileAsync(log);
        }

        public async Task LogAsync(HttpContext httpContext, Exception exception)
        {
            var log = new CoreLog()
            {
                DateTime = DateTime.Now,
                ErrorMessage = exception.Message,
                Exception = exception,
                RequsetPath = httpContext.Request.Path,
                UserAgent = "?",
                UserIp = "?"

            };
            await WriteFileAsync(log);
        }

        public async Task LogAsync(HttpContext httpContext, string Message)
        {
            var log = new CoreLog()
            {
                DateTime = DateTime.Now,
                ErrorMessage = Message,
                Exception = null,
                RequsetPath = httpContext.Request.Path,
                UserAgent = "?",
                UserIp = "?"

            };
            await WriteFileAsync(log);
        }


        public void Log(CoreLog log)
        {
            WriteFile(log);
        }

        public async Task LogAsync(CoreLog log)
        {
            await WriteFileAsync(log);
        }

        readonly string DiskPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

        const string FileName = "log.txt";

        string Getpath()
        {
            return Path.Combine(DiskPath, FileName);
        }

        async Task WriteFileAsync(CoreLog log)
        {
            string data = Newtonsoft.Json.JsonConvert.SerializeObject(log);
            if (!Directory.Exists(DiskPath))
            {
                Directory.CreateDirectory(DiskPath);
            }
            var filepath = Path.Combine(DiskPath, FileName);
            if (!File.Exists(filepath))
            {
                using (StreamWriter sw = File.CreateText(filepath))
                {
                    await sw.WriteLineAsync($"{data},");

                }
            }
            else
            {
                using (StreamWriter sw = File.AppendText(filepath))
                {
                    await sw.WriteLineAsync($"{data},");
                }
            }


        }

        void WriteFile(CoreLog log)
        {

            string data = JsonConvert.SerializeObject(log);


            if (!File.Exists(Getpath()))
            {
                using (StreamWriter sw = File.CreateText(Getpath()))
                {
                    sw.WriteLine($"{data},");

                }
            }
            else
            {
                using (StreamWriter sw = File.AppendText(Getpath()))
                {
                    sw.WriteLine($"{data},");
                }
            }
        }

        public List<CoreLog> ReadLogs()
        {

            if (!File.Exists(Getpath()))
            {
                return null;
            }
            else
            {
                var data = File.ReadAllText(Getpath());
                data = data.TrimEnd(',');
                data = $"[{data}]";
                List<CoreLog> logs = JsonConvert.DeserializeObject<List<CoreLog>>(data);
                return logs;
            }
        }

        public async Task<List<CoreLog>> ReadLogsAsync()
        {

            if (!File.Exists(Getpath()))
            {
                return null;
            }
            else
            {
                var data = File.ReadAllText(Getpath());
                data = data.TrimEnd(',');
                data = $"[{data}]";
                List<CoreLog> logs = JsonConvert.DeserializeObject<List<CoreLog>>(data);
                return logs;
            }
        }

        public void ClearLogs()
        {
            File.Delete(Getpath());
        }
    }
}

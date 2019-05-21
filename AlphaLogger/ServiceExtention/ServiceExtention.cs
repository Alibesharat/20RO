using AlphaCoreLogger;
using Microsoft.AspNetCore.Builder;

namespace Microsoft.Extensions.DependencyInjection
{
   public static class ServiceExtention
    {
        /// <summary>
        /// تزریق دیسک لاگر به برنامه
        /// </summary>
        /// <param name="services"></param>
        /// <param name="ConnectionString"></param>
        /// <returns></returns>
        public static IServiceCollection AddDiskLooger(this IServiceCollection services)
        {
            var service = services.AddSingleton<ICoreLogger, DiskLogger>();
           
            return service;
        }


       
        public static void  UseAlphaCoreLogger(this IApplicationBuilder app,string path)
        {
            app.UseMiddleware<CoreLoggerMiddleware>(path);
        }


        //public  static IServiceCollection AddDatabseLogger(this IServiceCollection services,string ConnectionString)
        //{
        //    var service = services.AddSingleton<ICoreLogger, DiskLogger>();
        //    return service;
        //}
    
    }
}

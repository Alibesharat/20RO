using DAL;
using Microsoft.EntityFrameworkCore;


namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceExtention
    {
        /// <summary>
        /// تزریق کانتکست به برنامه
        /// </summary>
        /// <param name="services"></param>
        /// <param name="ConnectionString"></param>
        /// <returns></returns>
        public static IServiceCollection AddTaxiContext(this IServiceCollection services, string ConnectionString)
        {

            var service = services.AddDbContext<TaxiContext>(options =>
              options.UseSqlServer(ConnectionString, opt => { opt.UseRowNumberForPaging(); }));
            return service;
        }

        

    }
}

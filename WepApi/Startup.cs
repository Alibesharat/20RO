using Kavenegar.Core.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NotifCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace WepApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTaxiContext(Configuration.GetConnectionString("Con"));
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Version = "v1",
                    Title = "My API",
                    Description = "My Web API",
                    TermsOfService = new Uri("http://taxi.ir"),
                    Contact = new Microsoft.OpenApi.Models.OpenApiContact
                    {
                        Name = "Ali Besharati",
                        Email = "",
                        Url = new Uri("http://taxi.ir"),
                    },


                });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);



            });
            services.AddDiskLooger();
            services.AddSingleton<ISMS<List<SendResult>>, kaveSMS>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {


            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                    c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);

                });
            }
            else
            {
                //// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                //app.UseHsts();
            }
          
            app.UseStaticFiles();
          
           
            //app.UseHttpsRedirection();
            app.UseMvc(opt=>{
                opt.MapRoute("Default",
                    template: "{controller=swagger}/{action=Index}");


            });
        }
    }
}

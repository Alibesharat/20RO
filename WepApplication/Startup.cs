using Kavenegar.Core.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NotifCore;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;

namespace WepApplication
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
            services.Configure<CookiePolicyOptions>(options =>
             {
                 // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                 options.CheckConsentNeeded = context => true;
                 options.MinimumSameSitePolicy = SameSiteMode.None;
                 options.Secure = CookieSecurePolicy.Always;
             });
            services.AddAuthentication(option =>
            {
                option.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;


            }).AddCookie(option =>
            {
                option.AccessDeniedPath = "/Account/Denied";
                option.LoginPath = "/Account/UserChalenge";
                option.SlidingExpiration = true;
                option.Cookie.Name = "btscores";
                option.ExpireTimeSpan = TimeSpan.FromDays(15);

            });

          
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddHsts(options =>
            {
                options.Preload = true;
                options.IncludeSubDomains = true;
                options.MaxAge = TimeSpan.FromDays(60);
               
            });

            services.AddHttpsRedirection(options =>
            {
                options.RedirectStatusCode = StatusCodes.Status302Found;
            });
            services.AddSingleton<ISMS<List<SendResult>>, kaveSMS>();
            services.AddDistributedSqlServerCache(opt => {
                opt.ConnectionString = Configuration.GetConnectionString("cashdb");
                opt.ExpiredItemsDeletionInterval = TimeSpan.FromMinutes(5);
                opt.SchemaName = "dbo";
                opt.TableName = "Cache";
                });
           
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            }

            app.UseHsts();
            app.UseHttpsRedirection();
            app.UseAuthentication();

            app.UseStaticFiles(new StaticFileOptions()
            {
                OnPrepareResponse = context =>
                {
                    context.Context.Response.Headers["Cache-Control"] =
                            "private, max-age=43200";

                    context.Context.Response.Headers["Expires"] =
                            DateTime.UtcNow.AddDays(30).ToString("R");
                }
            });
            app.UseCookiePolicy();
            app.Use(async (context, next) =>
            {
                // For GetTypedHeaders, add: using Microsoft.AspNetCore.Http;
                context.Response.GetTypedHeaders().CacheControl =
                    new Microsoft.Net.Http.Headers.CacheControlHeaderValue()
                    {
                        Public = true,
                        MaxAge = TimeSpan.FromDays(30)
                    };
                context.Response.Headers[Microsoft.Net.Http.Headers.HeaderNames.Vary] =
                    new string[] { "Accept-Encoding" };

                await next();
            });
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

        }
    }
}

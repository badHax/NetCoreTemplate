using BotDetect.Web;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NetCoreTemplate.Web.Extensions;
using Serilog;
using System;
using NetCoreTemplate.DAL.API;

namespace NetCoreTemplate.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment hostEnv)
        {
            Configuration = configuration;

            // Create the logger
            Environment.SetEnvironmentVariable("BASEDIR",hostEnv.ContentRootPath);
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(Configuration)
                .CreateLogger();
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
            });

            services.AddMvc(options => options.Filters.Add<HandleAppError>())
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddNToastNotifyNoty(new NToastNotify.NotyOptions() // add toast messages
                {
                    ProgressBar = false,
                    Timeout = 30000,
                    Theme = "metroui",
                    Modal = true,
                    CloseWith = new []{"button"},
                    Layout = "topCenter"
                });

           //lowercase url shown in browser
            services.AddRouting(options => options.LowercaseUrls = true);

            //add session services
            services.AddSession(options => {
                options.IdleTimeout = TimeSpan.FromMinutes(20);
                options.Cookie.IsEssential = true;
            });

            // access to httpcontext outside the controllers
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddMemoryCache();

            services.AddLogging(loggingBuilder => 
                loggingBuilder.AddSerilog(dispose: true, logger:Log.Logger));

            // access to appsettings.json
            services.AddSingleton(Configuration);

            services.AddSingleton<NetCoreTemplate.DAL.API.IServiceProvider, APIClient>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            //toast middleware
            app.UseNToastNotify();

            //session middleware
            app.UseSession();

            // Captcha middleware
            app.UseCaptcha(Configuration);

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}

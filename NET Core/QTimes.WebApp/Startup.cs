using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using QTimes.core.dal;
using QTimes.core.dal.Repositories;
using QTimes.WebApp.Infrastructure;
using Microsoft.AspNetCore.Identity.UI.Services;
using iVeew.common.api.Infrastructure;
using System.Collections.Generic;
using System.Reflection;
using SeatQ.core.dal.Repositories;
using System;
using System.Linq;
using iVeew.common.api.Queries;
using IVeew.common.dal;
using QTimes.core.dal.Models;
using Microsoft.AspNetCore.ResponseCompression;
using System.IO.Compression;

namespace QTimes.WebApp
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
            AddDependencies(services);
            services.AddControllersWithViews()
                .AddRazorRuntimeCompilation();
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));

            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>();
            //services.AddControllersWithViews();
            QTimesContext.SetConnectionString(Configuration.GetConnectionString("DefaultConnection"));
            var mvcBuilder = services.AddControllersWithViews(options =>
            {
                options.Filters.Add(new ViewBagConfigurationActionFilter(Configuration));
            });
#if DEBUG
            mvcBuilder.AddRazorRuntimeCompilation();
#endif

            services.AddResponseCompression();

            services.Configure<BrotliCompressionProviderOptions>(options =>
            {
                options.Level = CompressionLevel.Fastest;
            });

            services.AddRazorPages();
            services.AddApplicationInsightsTelemetry(Configuration["APPINSIGHTS_INSTRUMENTATIONKEY"]);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseCookiePolicy();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "Concert",
                    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }

        private void AddDependencies(IServiceCollection services)
        {
            foreach (var coreAssembly in GetCoreAssemblies())
            {
                var types = coreAssembly.GetTypes();
                RegisterCoreType(services, types);
            };
            services.AddScoped<IUserInfoRepo, UserInfoRepo>();
            services.AddScoped<IEmailSender, SendGridEmailSender>();
        }
        private void RegisterCoreType(IServiceCollection kernel, IEnumerable<Type> types)
        {
            foreach (var type in types)
            {
                var searchedRepositories = type.GetInterfaces().SingleOrDefault(x => x.IsGenericType &&
                    x.GetGenericTypeDefinition() == typeof(IGenericRepository<,>));
                if (null != searchedRepositories)
                    kernel.AddScoped(searchedRepositories, type);

            }
        }
        private static IEnumerable<Assembly> GetCoreAssemblies()
        {
            yield return Assembly.GetAssembly(typeof(ConcertRepo));
        }

    }
}

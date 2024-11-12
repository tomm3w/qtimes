using Hangfire;
using Hangfire.SqlServer;
using iVeew.common.api;
using iVeew.common.api.Commands;
using iVeew.common.api.Queries;
using IVeew.common.dal;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using QTimes.api.Infrastructure;
using QTimes.api.Infrastructure.NexmoMessaging;
using QTimes.api.Queries.Reservation.GetConcertEventMessages;
using QTimes.api.Services;
using QTimes.core.dal;
using QTimes.core.dal.Models;
using RestSharp;
using SeatQ.core.dal.Repositories;
using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Text;

namespace QTimes.api
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
            services.AddCors();
            services.AddHangfire(configuration => configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(Configuration.GetConnectionString("HangfireConnection"), new SqlServerStorageOptions
                {
                    CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                    SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                    QueuePollInterval = TimeSpan.Zero,
                    UseRecommendedIsolationLevel = true,
                    DisableGlobalLocks = true
                }));

            // Add the processing server as IHostedService
            services.AddHangfireServer();
            services.AddResponseCompression();

            services.Configure<BrotliCompressionProviderOptions>(options =>
            {
                options.Level = CompressionLevel.Fastest;
            });
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));


            services.AddIdentity<IdentityUser, IdentityRole>()
        .AddEntityFrameworkStores<ApplicationDbContext>();

            AddDependencies(services);
            services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.DictionaryKeyPolicy = null;
                options.JsonSerializerOptions.PropertyNamingPolicy = null;
                options.JsonSerializerOptions.Converters.Add(new TimeSpanConverter());
            });
            services.AddStackExchangeRedisCache(options => options.Configuration = this.Configuration.GetConnectionString("redisServerUrl"));
            QTimesContext.SetConnectionString(Configuration.GetConnectionString("DefaultConnection"));
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            });

            services.AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(o =>
            {
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidAudience = Configuration["JWT:ValidAudience"],
                    ValidIssuer = Configuration["JWT:ValidIssuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Secret"]))
                };


                o.Events = new JwtBearerEvents()
                {
                    OnAuthenticationFailed = e =>
                    {
                        return e.Response.WriteAsync("An error occurred processing your authentication.");
                    },
                    OnForbidden = f =>
                    {
                        return f.Response.WriteAsync("An error occurred processing your authentication.");
                    },
                };
            });
            services.AddApplicationInsightsTelemetry(Configuration["APPINSIGHTS_INSTRUMENTATIONKEY"]);

        }

        private void AddDependencies(IServiceCollection services)
        {
            foreach (var coreAssembly in GetCoreAssemblies())
            {
                var types = coreAssembly.GetTypes();
                RegisterCoreType(services, types);
            };
            services.AddScoped<IApplicationFacade, ApplicationFacade>();
            services.AddScoped<IMessaging, Messaging>();
            services.AddScoped<IPasstrekClient, PasstrekClient>();
            services.AddScoped<IBlobService, BlobService>();
            services.AddScoped<IRestClient, RestClient>();
            services.AddHttpContextAccessor();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseHangfireDashboard();
            AutoMapperConfig.Initialize();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseCors(
                options => options
                .AllowAnyMethod()
                .AllowAnyOrigin()
                .AllowAnyHeader()
            );

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors();
            app.UseAuthentication();
            app.UseAuthorization();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }


        private void RegisterCoreType(IServiceCollection kernel, IEnumerable<Type> types)
        {
            foreach (var type in types)
            {
                var searchedQuery = type.GetInterfaces().SingleOrDefault(x => x.IsGenericType &&
                    x.GetGenericTypeDefinition() == typeof(IQuery<,>));

                var searchedCommand = type.GetInterfaces().SingleOrDefault(x => x.IsGenericType &&
                    (x.GetGenericTypeDefinition() == typeof(ICommand<>) || x.GetGenericTypeDefinition() == typeof(IAsyncCommand<,>)));

                var searchedRepositories = type.GetInterfaces().SingleOrDefault(x => x.IsGenericType &&
                    x.GetGenericTypeDefinition() == typeof(IGenericRepository<,>));
                if (null != searchedRepositories)
                    kernel.AddScoped(searchedRepositories, type);

                if (null != searchedQuery)
                    kernel.AddScoped(searchedQuery, type);

                if (null != searchedCommand)
                    kernel.AddScoped(searchedCommand, type);

            }
        }
        private static IEnumerable<Assembly> GetCoreAssemblies()
        {
            yield return Assembly.GetAssembly(typeof(GetConcertEventMessagesQuery));
            yield return Assembly.GetAssembly(typeof(ConcertRepo));
        }
    }
}

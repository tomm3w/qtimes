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
using Microsoft.OpenApi.Models;
using QTimes.api;
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
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Text;

namespace QTimes_3v.api
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
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "QTimes Api", Version = "v1" });

                // Define the Bearer token authentication scheme
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                // Add the Bearer token scheme to be applied globally
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
            });

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

            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.RoutePrefix = string.Empty; // To serve the Swagger UI at the app's root (e.g. http://localhost:<port>/)
            });


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
    //public class Startup
    //{
    //    public Startup(IConfiguration configuration)
    //    {
    //        Configuration = configuration;
    //    }

    //    public IConfiguration Configuration { get; }

    //    // This method gets called by the runtime. Use this method to add services to the container.
    //    public void ConfigureServices(IServiceCollection services)
    //    {
    //        services.AddControllers();
    //    }

    //    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    //    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    //    {
    //        if (env.IsDevelopment())
    //        {
    //            app.UseDeveloperExceptionPage();
    //        }

    //        app.UseHttpsRedirection();

    //        app.UseRouting();

    //        app.UseAuthorization();

    //        app.UseEndpoints(endpoints =>
    //        {
    //            endpoints.MapControllers();
    //        });
    //    }
    //}
}

using Hangfire;
using iVeew.common.api;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using QTimes.core.dal.Models;
using QTimes.core.dal;
using RestSharp;
using System.IO.Compression;
using System.Text;
using Hangfire.SqlServer;
using iVeew.common.api.Commands;
using iVeew.common.api.Queries;
using IVeew.common.dal;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using QTimes.api.Infrastructure;
using QTimes.api.Infrastructure.NexmoMessaging;
using QTimes.api.Queries.Reservation.GetConcertEventMessages;
using QTimes.api.Services;
using SeatQ.core.dal.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using QTimes.api;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

ConfigureServices(builder);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("AllowSpecificOrigin");
app.UseAuthentication();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();



void RegisterCoreType(IServiceCollection kernel, IEnumerable<Type> types)
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
static IEnumerable<Assembly> GetCoreAssemblies()
{
    yield return Assembly.GetAssembly(typeof(GetConcertEventMessagesQuery));
    yield return Assembly.GetAssembly(typeof(ConcertRepo));
}
void ConfigureServices(WebApplicationBuilder builder)
{
    //builder.Services.AddCors();
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowSpecificOrigin",
            policyBuilder =>
            {
                policyBuilder.WithOrigins("https://localhost:44347")  // Allow requests from your frontend origin
                             .AllowAnyHeader()                        // Allow any headers
                             .AllowAnyMethod();                       // Allow all HTTP methods (GET, POST, etc.)
            });
    });
    builder.Services.AddHangfire(configuration => configuration
        .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
        .UseSimpleAssemblyNameTypeSerializer()
        .UseRecommendedSerializerSettings()
        .UseSqlServerStorage(builder.Configuration.GetConnectionString("HangfireConnection"), new SqlServerStorageOptions
        {
            CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
            SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
            QueuePollInterval = TimeSpan.Zero,
            UseRecommendedIsolationLevel = true,
            DisableGlobalLocks = true
        }));

    // Add the processing server as IHostedService
    builder.Services.AddHangfireServer();
    builder.Services.AddResponseCompression();

    builder.Services.Configure<BrotliCompressionProviderOptions>(options =>
    {
        options.Level = CompressionLevel.Fastest;
    });
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(
            builder.Configuration.GetConnectionString("DefaultConnection")));


    builder.Services.AddIdentity<IdentityUser, IdentityRole>()
.AddEntityFrameworkStores<ApplicationDbContext>();
    AddDependencies(builder.Services);
    builder.Services.AddControllers().AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.DictionaryKeyPolicy = null;
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
        options.JsonSerializerOptions.Converters.Add(new TimeSpanConverter());
    });
    builder.Services.AddStackExchangeRedisCache(options => options.Configuration = builder.Configuration.GetConnectionString("redisServerUrl"));
    QTimesContext.SetConnectionString(builder.Configuration.GetConnectionString("DefaultConnection"));
    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    });

    builder.Services.AddAuthentication(o =>
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
            ValidAudience = builder.Configuration["JWT:ValidAudience"],
            ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"]))
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
    //var jwtSettings = builder.Configuration.GetSection("Jwt");

    //builder.Services.AddAuthentication(options =>
    //{
    //    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    //    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    //})
    //.AddJwtBearer(options =>
    //{
    //    options.TokenValidationParameters = new TokenValidationParameters
    //    {
    //        ValidateIssuer = true,
    //        ValidateAudience = true,
    //        ValidateLifetime = true,
    //        ValidateIssuerSigningKey = true,
    //        ValidIssuer = jwtSettings["Issuer"],
    //        ValidAudience = jwtSettings["Audience"],
    //        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]))
    //    };
    //});

    builder.Services.AddApplicationInsightsTelemetry(builder.Configuration["APPINSIGHTS_INSTRUMENTATIONKEY"]);

}

void AddDependencies(IServiceCollection services)
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
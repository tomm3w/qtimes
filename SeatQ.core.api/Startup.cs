using common.api.MultipartDataMediaFormatter;
using Hangfire;
using Hangfire.Annotations;
using Hangfire.Dashboard;
using IdentityServer3.AccessTokenValidation;
using Microsoft.Owin;
using Owin;
using SeatQ.core.api.Infrastructure;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens;
using System.Net;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

[assembly: OwinStartup(typeof(SeatQ.core.api.Startup))]
namespace SeatQ.core.api
{

    public class Startup
	{
		public void Configuration(IAppBuilder app)
		{
		    ConfigureTokenVlidation(app);
            AreaRegistration.RegisterAllAreas();

            var httpConfig = WebApiConfig.Configure();

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            NinjectHttpContainer.RegisterModules(NinjectHttpModules.Modules, httpConfig);
            AutoMapperConfig.Initialize();
            //app.UseClaimsTransformation(new ClaimsTransformer().Transform);
            app.UseWebApi(httpConfig);
			httpConfig.Filters.Add(new ValidateModelAttribute());
			httpConfig.Formatters.Add(new FormMultipartEncodedMediaTypeFormatter());

            Swashbuckle.Bootstrapper.Init(httpConfig);

			ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

			GlobalConfiguration.Configuration
			   .UseSqlServerStorage("SeatQHangfire");

			app.UseHangfireDashboard("/hangfire", new DashboardOptions()
			{
				Authorization = new[] { new HangFireAuthorizationFilter() }
			});

			app.UseHangfireDashboard();
			app.UseHangfireServer();

		}
	    private void ConfigureTokenVlidation(IAppBuilder app)
	    {
	        JwtSecurityTokenHandler.InboundClaimTypeMap = new Dictionary<string, string>();
	        app.UseIdentityServerBearerTokenAuthentication(new IdentityServerBearerTokenAuthenticationOptions
	        {
	            Authority = ConfigurationManager.AppSettings["Authority"],
	            ValidationMode = ValidationMode.Local,
	            RequiredScopes = new[] { "api" }

	        });
	    }

    }

	public class HangFireAuthorizationFilter : IDashboardAuthorizationFilter
	{
		public bool Authorize([NotNull] DashboardContext context)
		{
			//can add some more logic here...
			return true;
		}
	}
}
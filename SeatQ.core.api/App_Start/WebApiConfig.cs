using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
using Thinktecture.IdentityModel.WebApi.Authentication.Handler;

namespace SeatQ.core.api
{
    public static class WebApiConfig
    {
        public static HttpConfiguration Configure()
        {
            var config = new HttpConfiguration();

            config.EnableCors();
            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            //var con = CreateAuthenticationConfiguration();
            //config.MessageHandlers.Add(new AuthenticationHandler(con));
            //config.SuppressDefaultHostAuthentication();

            var corsAttr = new EnableCorsAttribute("*", "*", "*");
            config.EnableCors(corsAttr);

            return config;
        }

        private static AuthenticationConfiguration CreateAuthenticationConfiguration()
        {
            var authentication = new AuthenticationConfiguration
            {
                //ClaimsAuthenticationManager = new ClaimsTransformer().Transform(),
                RequireSsl = false,
                EnableSessionToken = true
            };

            #region IdentityServer JWT

            const string secret = "1fTiS2clmPTUlNcpwYzd5i4AEFJ2DEsd8TcUsllmaKQ=";
            authentication.AddJsonWebToken(
                issuer: "iDestn",
                audience: "users",
                signingKey: secret, scheme: "Bearer");
            #endregion

            return authentication;
        }
    }
}

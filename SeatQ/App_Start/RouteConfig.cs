using SeatQ.App_Start;
using System.Web.Mvc;
using System.Web.Routing;

namespace SeatQ
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // This will add the parameter "subdomain" to the route parameters
            //routes.Add(new SubdomainRoute());

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "SeatQ.Controllers" }
            );
            routes.MapMvcAttributeRoutes();
        }
    }
}
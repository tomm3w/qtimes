using System.Web.Mvc;

namespace QTimes.Areas.Concert
{
    public class ConcertAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Concert";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Concert_default",
                "Concert/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
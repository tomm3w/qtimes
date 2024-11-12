using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;

namespace QTimes.WebApp.Infrastructure
{
    public class ViewBagConfigurationActionFilter : ActionFilterAttribute
    {
        private readonly IConfiguration _configuration;
        public ViewBagConfigurationActionFilter(IConfiguration configuration) : base()
        {
            _configuration = configuration;
        }
        public override void OnResultExecuting(ResultExecutingContext context)
        {

            if (context.Controller is Controller)
            {
                var controller = context.Controller as Controller;
                controller.ViewBag.CoreApiEndpoint = _configuration["coreApiEndpoint"];
                controller.ViewBag.dlScanAppUrldlScanAppUrl = _configuration["dlScanAppUrldlScanAppUrl"];
            }

            base.OnResultExecuting(context);
        }
    }
}

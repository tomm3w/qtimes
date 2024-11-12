using System.Configuration;
using System.Web.Mvc;

namespace QTimes.Areas.Admin.Controllers
{
    public class BaseReservationController : Controller
    {
        public BaseReservationController()
        {
            ViewBag.CoreApiEndpoint = ConfigurationManager.AppSettings["coreApiEndpoint"];
        }
    }
}
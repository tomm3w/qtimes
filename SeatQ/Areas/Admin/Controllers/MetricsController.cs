using System.Web.Mvc;

namespace SeatQ.Areas.Admin.Controllers
{
    [Authorize(Roles = "Administrator,Regional Manager, User")]
    public class MetricsController : BaseAdminController
    {
        public ActionResult Index()
        {
            ViewBag.selectedMenu = @"Metrics";
            return View();
        }

    }
}

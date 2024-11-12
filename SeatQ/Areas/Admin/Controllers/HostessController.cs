using System.Web.Mvc;

namespace SeatQ.Areas.Admin.Controllers
{
     [Authorize(Roles = "Administrator, Regional Manager")]
    public class HostessController : BaseAdminController
    {
        public ActionResult Index()
        {
            ViewBag.selectedMenu = @"Hostess";
            return View();
        }

    }
}

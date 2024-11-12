using System.Web.Mvc;

namespace SeatQ.Areas.Admin.Controllers
{
    [Authorize(Roles = "Administrator, Regional Manager, User")]
    public class WaitListController : BaseAdminController
    {
        public ActionResult Index()
        {
            ViewBag.selectedMenu = @"Wait List";
            return View();
        }

        public ActionResult Hostess()
        {
            ViewBag.selectedMenu = @"Wait List";
            return View();
        }
    }
}

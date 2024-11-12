using System.Web.Mvc;

namespace SeatQ.Areas.Admin.Controllers
{
     [Authorize(Roles = "Administrator, Regional Manager")]
    public class ReturnGuestController : BaseAdminController
    {
        public ActionResult Index()
        {
            //ViewBag.selectedMenu = @"Return Guest";
            ViewBag.selectedMenu = @"Loyalty";
            return View();
        }

    }
}

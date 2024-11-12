using System.Web.Mvc;

namespace SeatQ.Areas.Admin.Controllers
{
    [Authorize(Roles = "Administrator, Regional Manager")]
    public class MessageController : BaseAdminController
    {
        public ActionResult Index()
        {
            ViewBag.selectedMenu = @"Messages";
            return View();
        }

    }
}

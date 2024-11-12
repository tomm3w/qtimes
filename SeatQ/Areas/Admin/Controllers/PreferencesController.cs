using System.Web.Mvc;

namespace SeatQ.Areas.Admin.Controllers
{
    [Authorize(Roles = "Administrator, Regional Manager")]
    public class PreferencesController : BaseAdminController
    {
        // GET: Admin/Preferences
        public ActionResult Index()
        {
            ViewBag.selectedMenu = @"Setting";
            return View();
        }
    }
}
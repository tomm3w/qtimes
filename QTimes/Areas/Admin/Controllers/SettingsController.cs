using System.Web.Mvc;

namespace QTimes.Areas.Admin.Controllers
{
    [Authorize(Roles = "Administrator, Regional Manager")]
    public class SettingsController : BaseReservationController
    {
        public ActionResult Account()
        {
            return View();
        }
        public ActionResult Preference()
        {
            return View();
        }
        public ActionResult Business()
        {
            return View();
        }
    }
}
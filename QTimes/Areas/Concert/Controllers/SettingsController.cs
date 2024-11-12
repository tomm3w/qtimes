using QTimes.Areas.Admin.Controllers;
using System.Web.Mvc;

namespace QTimes.Areas.Concert.Controllers
{
    [Authorize(Roles = "Administrator, Regional Manager")]
    public class SettingsController : BaseReservationController
    {
        public ActionResult Account()
        {
            return View();
        }
        public ActionResult Preferences()
        {
            return View();
        }
        public ActionResult Business()
        {
            return View();
        }
        public ActionResult Seatings()
        {
            return View();
        }
    }
}
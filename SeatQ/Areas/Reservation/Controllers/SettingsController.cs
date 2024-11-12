using System.Web.Mvc;

namespace SeatQ.Areas.Reservation.Controllers
{
    [Authorize(Roles = "Administrator, Regional Manager")]
    public class SettingsController : BaseReservationController
    {
        // GET: Reservation/Settings
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
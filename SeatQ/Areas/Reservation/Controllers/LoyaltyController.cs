using System.Web.Mvc;

namespace SeatQ.Areas.Reservation.Controllers
{
    [Authorize(Roles = "Administrator, Regional Manager")]
    public class LoyaltyController : BaseReservationController
    {
        // GET: Reservation/Reservation
        public ActionResult Index()
        {
            return View();
        }
    }
}
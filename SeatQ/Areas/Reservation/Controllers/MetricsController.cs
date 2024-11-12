using System.Web.Mvc;

namespace SeatQ.Areas.Reservation.Controllers
{
    [Authorize(Roles = "Administrator, Regional Manager")]
    public class MetricsController : BaseReservationController
    {
        // GET: Reservation/Metrics
        public ActionResult Index()
        {
            return View();
        }
    }
}
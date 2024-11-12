using System.Web.Mvc;

namespace QTimes.Areas.Admin.Controllers
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
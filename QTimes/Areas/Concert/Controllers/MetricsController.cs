using QTimes.Areas.Admin.Controllers;
using System.Web.Mvc;

namespace QTimes.Areas.Concert.Controllers
{
    [Authorize(Roles = "Administrator, Regional Manager")]
    public class MetricsController : BaseReservationController
    {
        // GET: Concert/Metrics
        public ActionResult Index()
        {
            return View();
        }
    }
}
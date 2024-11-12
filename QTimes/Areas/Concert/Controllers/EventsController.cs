using QTimes.Areas.Admin.Controllers;
using System.Web.Mvc;

namespace QTimes.Areas.Concert.Controllers
{
    [Authorize(Roles = "Administrator, Regional Manager")]
    public class EventsController : BaseReservationController
    {
        // GET: Concert/Events
        public ActionResult Index()
        {
            return View();
        }
    }
}
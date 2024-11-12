using QTimes.Areas.Admin.Controllers;
using System.Web.Mvc;

namespace QTimes.Areas.Concert.Controllers
{
    [AllowAnonymous]
    public class CheckInController : BaseReservationController
    {
        // GET: Concert/CheckIn
        public ActionResult Index(string id)
        {
            ViewBag.id = id;
            return View();
        }
    }
}
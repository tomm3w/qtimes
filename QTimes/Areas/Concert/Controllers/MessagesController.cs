using QTimes.Areas.Admin.Controllers;
using System.Web.Mvc;

namespace QTimes.Areas.Concert.Controllers
{
    [Authorize(Roles = "Administrator, Regional Manager")]
    public class MessagesController : BaseReservationController
    {
        // GET: Concert/Messages
        public ActionResult Index()
        {
            return View();
        }
    }
}
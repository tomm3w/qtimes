using QTimes.Areas.Admin.Controllers;
using System.Web;
using System.Web.Mvc;

namespace QTimes.Areas.Concert.Controllers
{
    [Authorize(Roles = "Administrator, Regional Manager")]
    public class AccountsController : BaseReservationController
    {
        // GET: Concert/Accounts
        public ActionResult Index()
        {
            HttpCookie cookie = Request.Cookies["concertid"];
            if (cookie != null)
                ViewBag.BusinessType = "Event";
            else
                ViewBag.BusinessType = "Business";

            return View();
        }
    }
}
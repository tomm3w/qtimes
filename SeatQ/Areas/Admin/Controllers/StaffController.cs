using System.Web.Mvc;

namespace SeatQ.Areas.Admin.Controllers
{
    [Authorize(Roles = "Administrator, Regional Manager")]
    public class StaffController : BaseAdminController
    {
        // GET: Admin/Staff
        public ActionResult Index()
        {
            ViewBag.selectedMenu = @"Staff";
            return View();
        }
    }
}
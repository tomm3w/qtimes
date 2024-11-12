using System.Web.Mvc;

namespace SeatQ.Areas.Admin.Controllers
{
    public class SeatingController : BaseAdminController
    {
        // GET: Admin/Seating
        public ActionResult Index()
        {
            ViewBag.selectedMenu = @"Seating";
            return View();
        }

        public ActionResult Layout()
        {
            ViewBag.selectedMenu = @"Seating";
            return View();
        }

        public ActionResult Table()
        {
            ViewBag.selectedMenu = @"Seating";
            return View();
        }
    }
}
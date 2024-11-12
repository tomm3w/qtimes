using System.Web.Mvc;


namespace SeatQ.Areas.Admin.Controllers
{
     [Authorize(Roles = "Administrator, Regional Manager")]
    public class TablesController : BaseAdminController
    {
        // GET: Admin/Tables
        public ActionResult Index()
        {
            ViewBag.selectedMenu = @"Setting";
            return View();
        }
    }
}
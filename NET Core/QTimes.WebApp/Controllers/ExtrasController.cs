using Microsoft.AspNetCore.Mvc;

namespace QTimes.Controllers
{
    public class ExtrasController : Controller
    {
        // GET: Extras
        public ActionResult Menu()
        {
            return View();
        }
        public ActionResult DL()
        {
            return View();
        }
        public ActionResult Voucher()
        {
            return View();
        }
        public ActionResult Seatmap()
        {
            return View();
        }
        public ActionResult Scanning()
        {
            return View();
        }
    }
}
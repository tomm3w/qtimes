using QTimes.Areas.Admin.Controllers;
using System.Web.Mvc;

namespace QTimes.Areas.m.Controllers
{
    public class ConcertController : BaseReservationController
    {
        public ActionResult Business(string id)
        {
            ViewBag.id = id;
            return View();
        }

        public ActionResult Reservation(string id)
        {
            ViewBag.id = id;
            return View();
        }

        public ActionResult Guests(string id)
        {
            ViewBag.id = id;
            return View();
        }

        public ActionResult Spotmap(string id)
        {
            ViewBag.id = id;
            return View();
        }
    }
}
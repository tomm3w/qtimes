using QTimes.Areas.Admin.Controllers;
using System.Web.Mvc;

namespace QTimes.Areas.m.Controllers
{
    public class ReservationController : BaseReservationController
    {
        public ActionResult Business(string id)
        {
            ViewBag.id = id;
            return View();
        }

        public ActionResult Reserve(string id)
        {
            ViewBag.id = id;
            return View();
        }

        public ActionResult ThankYou(string id)
        {
            ViewBag.id = id;
            return View();
        }

        public ActionResult Mirage(string id)
        {
            ViewBag.id = id;
            return View();
        }
    }
}
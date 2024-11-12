using System.Web.Mvc;

namespace SeatQ.Areas.Reservation.Controllers
{
    public class mController : BaseReservationController
    {
        // GET: Reservation/m
        public ActionResult Business(string id)
        {
            ViewBag.id = id;
            return View();
        }

        public ActionResult Waitlist(string id)
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
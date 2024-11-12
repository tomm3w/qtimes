using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SeatQ.Areas.Reservation.Controllers
{
    [Authorize(Roles = "Administrator, Regional Manager")]
    public class MessagesController : BaseReservationController
    {
        // GET: Reservation/Messages
        public ActionResult Index()
        {
            return View();
        }
    }
}
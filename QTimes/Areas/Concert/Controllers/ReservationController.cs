using QTimes.Areas.Admin.Controllers;
using System;
using System.Web;
using System.Web.Mvc;

namespace QTimes.Areas.Concert.Controllers
{
    [Authorize(Roles = "Administrator, Regional Manager")]
    public class ReservationController : BaseReservationController
    {
        // GET: Concert/Reservation
        public ActionResult Index(string id)
        {
            if (Request.Cookies["businessid"] != null)
            {
                var c = new HttpCookie("businessid");
                c.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(c);
            }

            HttpCookie cookie = new HttpCookie("concertid", id);
            cookie.Expires = DateTime.Now.AddDays(1);
            Response.Cookies.Add(cookie);
            return View();
        }
    }
}
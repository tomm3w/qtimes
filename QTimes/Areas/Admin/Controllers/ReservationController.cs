using System;
using System.Web;
using System.Web.Mvc;

namespace QTimes.Areas.Admin.Controllers
{
    [Authorize(Roles = "Administrator, Regional Manager")]
    public class ReservationController : BaseReservationController
    {
        // GET: Admin/Reservation
        public ActionResult Index(string id)
        {
            if (Request.Cookies["concertid"] != null)
            {
                var c = new HttpCookie("concertid");
                c.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(c);
            }

            HttpCookie cookie = new HttpCookie("businessid", id);
            cookie.Expires = DateTime.Now.AddDays(1);
            Response.Cookies.Add(cookie);

            return View();
        }
    }
}
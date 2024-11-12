using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;

namespace QTimes.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    public class ReservationController : BaseReservationController
    {
        public ReservationController(IConfiguration configuration) : base(configuration)
        {

        }
        // GET: Admin/Reservation
        public ActionResult Index(string id)
        {
            if (Request.Cookies["concertid"] != null)
            {
                CookieOptions option = new CookieOptions();
                option.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Append("concertid", Request.Cookies["concertid"], option);
            }

            if (!string.IsNullOrEmpty(id))
            {
                CookieOptions optionBusi = new CookieOptions();
                optionBusi.Expires = DateTime.Now.AddDays(1);
                Response.Cookies.Append("businessid", id, optionBusi);
            }

            return View();
        }
    }
}
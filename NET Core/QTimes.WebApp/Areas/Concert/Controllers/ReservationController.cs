using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using QTimes.Areas.Admin.Controllers;
using System;

namespace QTimes.Areas.Concert.Controllers
{
    [Authorize]
    [Area("Concert")]
    public class ReservationController : BaseReservationController
    {
        public ReservationController(IConfiguration configuration) : base(configuration)
        {

        }
        // GET: Concert/Reservation
        public ActionResult Index(string id)
        {
            if (Request.Cookies["businessid"] != null)
            {
                CookieOptions option = new CookieOptions();
                option.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Append("businessid", Request.Cookies["businessid"], option);
            }

            if (!string.IsNullOrEmpty(id))
            {
                CookieOptions optionBusi = new CookieOptions();
                optionBusi.Expires = DateTime.Now.AddDays(1);
                Response.Cookies.Append("concertid", id, optionBusi);
            }
            return View();
        }
    }
}
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using QTimes.Areas.Admin.Controllers;
using System;

namespace QTimes.Areas.Concert.Controllers
{
    [AllowAnonymous]
    [Area("Concert")]
    public class CheckInController : BaseReservationController
    {
        public CheckInController(IConfiguration configuration) : base(configuration)
        {

        }

        [Route("checkin/concerts/{concertId:Guid}/events/{eventId:Guid}")]
        public ActionResult CheckIn(Guid concertId, Guid eventId)
        {
            ViewBag.concertId = concertId;
            ViewBag.eventId = eventId;
            return View();
        }
    }
}
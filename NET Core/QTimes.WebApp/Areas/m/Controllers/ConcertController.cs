using IVeew.common.dal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using QTimes.Areas.Admin.Controllers;
using QTimes.core.dal.Models;
using System;
using System.Linq;

namespace QTimes.Areas.m.Controllers
{
    [Area("m")]
    public class ConcertController : BaseReservationController
    {
        private readonly IGenericRepository<QTimesContext, ConcertEventReservation> _reservationRepo;

        public ConcertController(IGenericRepository<QTimesContext, ConcertEventReservation> reservationRepo,
            IConfiguration configuration) : base(configuration)
        {
            _reservationRepo = reservationRepo;
        }
        [Route("m/concerts/{concertId:Guid}/events/{eventId:Guid}")]
        public ActionResult Events(Guid concertId, Guid eventId)
        {
            ViewBag.concertId = concertId;
            ViewBag.eventId = eventId;
            return View();
        }
        [Route("m/concerts/{concertId:Guid}/events/{eventId:Guid}/reservation")]
        public ActionResult Reservation(Guid concertId, Guid eventId)
        {
            ViewBag.concertId = concertId;
            ViewBag.eventId = eventId;
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

        public ActionResult ThankYou(int id)
        {
            ViewBag.id = id;
            var reservation = _reservationRepo.FindBy(x => x.Id == id).FirstOrDefault();
            if (reservation != null && !string.IsNullOrWhiteSpace(reservation.PassUrl))
            {
                ViewBag.PassUrl = reservation.PassUrl;
            }
            return View();
        }

        public ActionResult Expired()
        {
            return View();
        }

        public ActionResult Confirmation(int id)
        {
            var reservation = _reservationRepo.FindBy(x => x.Id == id).FirstOrDefault();
            if (reservation == null)
                return NotFound();

            ViewBag.webserviceURL = _configuration.GetValue<string>("coreApiEndpoint");
            return View(reservation);
        }
    }
}
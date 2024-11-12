using IVeew.common.dal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using QTimes.Areas.Admin.Controllers;
using QTimes.core.dal.Models;
using System.Linq;

namespace QTimes.Areas.m.Controllers
{
    [Area("m")]
    public class ReservationController : BaseReservationController
    {
        private readonly IGenericRepository<QTimesContext, Reservation> _reservationRepo;
        public ReservationController(IGenericRepository<QTimesContext, Reservation> reservationRepo, IConfiguration configuration) : base(configuration)
        {
            _reservationRepo = reservationRepo;
        }

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

        public ActionResult Mirage(string id)
        {
            ViewBag.id = id;
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
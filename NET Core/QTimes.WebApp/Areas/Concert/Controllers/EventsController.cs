using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using QTimes.Areas.Admin.Controllers;

namespace QTimes.Areas.Concert.Controllers
{
    [Authorize]
    [Area("Concert")]
    public class EventsController : BaseReservationController
    {
        public EventsController(IConfiguration configuration) : base(configuration)
        {

        }
        // GET: Concert/Events
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Preferences(string id)
        {
            ViewBag.id = id;
            return View();
        }

        public ActionResult Seatings(string id)
        {
            ViewBag.id = id;
            return View();
        }

        public ActionResult Business(string id)
        {
            ViewBag.id = id;
            return View();
        }
    }
}
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using QTimes.Areas.Admin.Controllers;

namespace QTimes.Areas.Concert.Controllers
{
    [Authorize]
    [Area("Concert")]
    public class SettingsController : BaseReservationController
    {
        public SettingsController(IConfiguration configuration) : base(configuration)
        {

        }
        public ActionResult Account()
        {
            return View();
        }
        public ActionResult Preferences()
        {
            return View();
        }
        public ActionResult Business()
        {
            return View();
        }
        public ActionResult Seatings()
        {
            return View();
        }
    }
}
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace QTimes.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    public class SettingsController : BaseReservationController
    {
        public SettingsController(IConfiguration configuration) : base(configuration)
        {

        }
        public ActionResult Account()
        {
            return View();
        }
        public ActionResult Preference()
        {
            return View();
        }
        public ActionResult Business()
        {
            return View();
        }
    }
}
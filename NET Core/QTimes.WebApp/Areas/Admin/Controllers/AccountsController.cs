using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace QTimes.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    public class AccountsController : BaseReservationController
    {
        public AccountsController(IConfiguration configuration) : base(configuration)
        {
        }

        // GET: Concert/Accounts
        public ActionResult Index()
        {
            var cookies = Request.Cookies["concertid"];
            if (cookies != null)
                ViewBag.BusinessType = "Event";
            else
                ViewBag.BusinessType = "Business";

            ViewBag.CoreApiEndpoint = _configuration["coreApiEndpoint"];
            return View();
        }
    }
}
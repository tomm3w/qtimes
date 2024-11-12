using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace QTimes.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    public class MessagesController : BaseReservationController
    {
        public MessagesController(IConfiguration configuration) : base(configuration)
        {

        }
        // GET: Reservation/Messages
        public ActionResult Index()
        {
            return View();
        }
    }
}
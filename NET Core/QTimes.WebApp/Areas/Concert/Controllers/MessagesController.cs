using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using QTimes.Areas.Admin.Controllers;


namespace QTimes.Areas.Concert.Controllers
{
    [Area("Concert")]
    [Authorize]
    public class MessagesController : BaseReservationController
    {
        public MessagesController(IConfiguration configuration) : base(configuration)
        {

        }
        // GET: Concert/Messages
        public ActionResult Index()
        {
            return View();
        }
    }
}
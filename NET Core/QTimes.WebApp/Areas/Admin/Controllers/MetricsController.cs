using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace QTimes.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    public class MetricsController : BaseReservationController
    {
        public MetricsController(IConfiguration configuration) : base(configuration)
        {

        }
        // GET: Reservation/Metrics
        public ActionResult Index()
        {
            return View();
        }
    }
}
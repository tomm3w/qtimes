using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using QTimes.Areas.Admin.Controllers;

namespace QTimes.Areas.Concert.Controllers
{
    [Area("Concert")]
    [Authorize]
    public class MetricsController : BaseReservationController
    {
        public MetricsController(IConfiguration configuration) : base(configuration)
        {

        }
        // GET: Concert/Metrics
        public ActionResult Index()
        {
            return View();
        }
    }
}
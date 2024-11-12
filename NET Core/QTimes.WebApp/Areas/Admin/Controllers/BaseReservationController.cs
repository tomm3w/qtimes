using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace QTimes.Areas.Admin.Controllers
{

    public class BaseReservationController : Controller
    {
        protected IConfiguration _configuration;
        public BaseReservationController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
    }
}
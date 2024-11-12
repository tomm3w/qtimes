using System.Configuration;
using System.Web.Mvc;

namespace SeatQ.Areas.Reservation.Controllers
{
    public class BaseReservationController : Controller
    {
        public BaseReservationController()
        {
            ViewBag.CoreApiEndpoint = ConfigurationManager.AppSettings["coreApiEndpoint"];
        }
    }
}
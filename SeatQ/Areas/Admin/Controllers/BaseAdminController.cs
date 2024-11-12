
using SeatQ.core.dal.Infrastructure.Repositories;
using SeatQ.core.dal.Models;
using System.Configuration;
using System.Web.Mvc;

namespace SeatQ.Areas.Admin.Controllers
{
    public class BaseAdminController : Controller
    {
        public BaseAdminController()
        {
            AccountInfoRepository userRepository = new AccountInfoRepository();
            UserProfile usr = userRepository.GetUserByUserName(Authentication.Web.Security.CurrentUserName);
            ViewBag.User = usr;
            ViewBag.CoreApiEndpoint = ConfigurationManager.AppSettings["coreApiEndpoint"];
            ViewBag.CORE_API_URL = ConfigurationManager.AppSettings["CORE_API_URL"];
            if (usr != null)
            {
                ViewBag.rcid = usr.RestaurantChainId;
                ViewBag.rid = usr.RestaurantId;
            }
        }
    }
}

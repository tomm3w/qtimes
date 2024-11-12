using System;
using System.Configuration;
using System.Web;
using System.Web.Mvc;
using SeatQ.core.dal.Infrastructure.Repositories;
using SeatQ.core.dal.Models;

namespace SeatQ.Areas.Mobile.Controllers
{
    public class WaitlistController : Controller
    {
        // GET: Mobile/Waitlist

        //[Route("{BusinessId}/{DeviceId}")]
        public ActionResult Index(int BusinessId, string DeviceId)
        {
            var usr = new AccountInfoRepository().GetRestaurantByBusinessId(BusinessId);
            if (usr == null)
                return HttpNotFound();

            ViewBag.BusinessName = usr.BusinessName;
            ViewBag.LogoPath = ContentAbsUrl(usr.LogoPath);
            ViewBag.CoreApiEndpoint = ConfigurationManager.AppSettings["coreApiEndpoint"];
            ViewBag.BusinessId = BusinessId;
            ViewBag.DeviceId = DeviceId;
            return View();
        }

        public string ContentAbsUrl(string relativeContentPath)
        {
            Uri contextUri = Request.Url;

            var baseUri = ConfigurationManager.AppSettings["coreApiEndpoint"].Replace("api/", "");
            if (relativeContentPath == null)
            {
                return baseUri;
            }
            return string.Format("{0}{1}", baseUri, VirtualPathUtility.ToAbsolute(relativeContentPath));
        }

    }
}
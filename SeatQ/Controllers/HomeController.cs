using Core.Attributes;
using Core.Exceptions;
using Core.Web;
using SeatQ.Areas.Admin.Models;
using System.Threading;
using System.Web.Mvc;

namespace SeatQ.Controllers
{
    public class HomeController : WebServiceBaseController
    {
        public ActionResult Index()
        {
            /*UserHelper.Role role = UserHelper.GetCurrentUserRole();
            if (User.Identity.IsAuthenticated)
            {
                if (role == UserHelper.Role.Administrator || role == UserHelper.Role.SuperAdministrator)
                {
                    return RedirectToAction("Index", "Metrics", new { area = "Admin" });
                }
                else if (role == UserHelper.Role.Hostess)
                {
                    
                }
                else
                {

                }
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }*/

            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            return View();
        }

        public ActionResult Indexold()
        {
            return View();
        }

        [WebserviceExecption]
        [AllowAnonymous]
        public JsonResult ContactSupport(ContactModel model)
        {
            ErrorCode errorCode = ErrorCode.UnKnownError;
            if(ModelState.IsValid)
            {
                new Thread(() =>
                {
                    new SeatQ.Helpers.EmailHelper().ContactSupport(model);
                }).Start();

                return GetJSON(new { status = "ok" });
            }

            throw new InvalidPostDataException { ErrorCode = errorCode, ModelState = ModelState };
        }
    }
}

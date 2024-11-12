using Core.Exceptions;
using Core.Web;
using SeatQ.Areas.Admin.Models;
using System.Threading;
using System.Web.Mvc;

namespace QTimes.Controllers
{
    public class HomeController : WebServiceBaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public JsonResult ContactSupport(ContactModel model)
        {
            ErrorCode errorCode = ErrorCode.UnKnownError;
            if (ModelState.IsValid)
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
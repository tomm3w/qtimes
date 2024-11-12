using iVeew.core.Web;
using iVeew.Core.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SeatQ.Areas.Admin.Models;
using System.Threading;

namespace QTimes.WebApp
{
    public class HomeController : WebServiceBaseController
    {
        private readonly IEmailSender _emailSender;
        private readonly IConfiguration _configuration;
        public HomeController(IEmailSender emailSender, IConfiguration configuration)
        {
            _emailSender = emailSender;
            _configuration = configuration;
        }
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

                string message = $"Please follow for business setup.<br><br>Name: {model.Name}<br/>Business Name: {model.BusinessName}<br/>Email: {model.Email}<br/>Phone: {model.Phone}<br/>Message: {model.Message}";
                _emailSender.SendEmailAsync(_configuration["contactEmail"], "Client contact", message).GetAwaiter().GetResult();
                return GetJSON(new { status = "ok" });
            }

            throw new InvalidPostDataException { ErrorCode = errorCode, ModelState = ModelState };
        }
    }
}
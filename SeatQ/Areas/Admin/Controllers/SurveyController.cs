using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SeatQ.Areas.Admin.Controllers
{
     [Authorize(Roles = "Administrator, Regional Manager")]
    public class SurveyController : BaseAdminController
    {
        public ActionResult Index()
        {
            ViewBag.selectedMenu = @"Survey";
            return View();
        }

    }
}

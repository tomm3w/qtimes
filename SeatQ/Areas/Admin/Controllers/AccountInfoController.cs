﻿using System.Web.Mvc;

namespace SeatQ.Areas.Admin.Controllers
{
    [Authorize(Roles = "Administrator, Regional Manager")]
    public class AccountInfoController : BaseAdminController
    {
        public ActionResult Index()
        {
            ViewBag.selectedMenu = @"Setting";
            return View();
        }

    }
}

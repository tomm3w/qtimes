using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Routing;
using QTimes.core.dal.Enums;
using SeatQ.Areas.Admin.Models;
using System.Collections.Generic;

namespace SeatQ.Helpers
{
    public class MenuHelper
    {
        //public MenuListModel GetMenu()
        //{
        //    UrlHelper Url = new UrlHelper(HttpContext.Request.RequestContext);
        //    MenuListModel menu = new MenuListModel();
        //    AccountInfoRepository userRepository = new AccountInfoRepository();
        //    UserProfile usr = userRepository.GetUserByUserName(Authentication.Web.Security.CurrentUserName);
        //    string role = (usr.RoleName == null) ? "user" : usr.RoleName.ToLower();

        //    if (role == "administrator" || role == "regional manager")
        //    {
        //        menu.Menus = new List<MenuModel>
        //        {
        //            new MenuModel{Name = "Wait List",Class="wait-icon", URL = Url.Action("index", "WaitList")},
        //            new MenuModel{Name = "Seating",Class="seat-icon", URL = Url.Action("index", "Seating")},
        //            new MenuModel{Name = "Loyalty",Class="loyal-icon",URL = Url.Action("index", "ReturnGuest")},
        //            new MenuModel{Name = "Messages",Class="message-icon", URL = Url.Action("index", "Message")},
        //            new MenuModel{Name = "Metrics", Class="metric-icon",URL = Url.Action("index", "Metrics")},
        //            //new MenuModel{Name = "Hostess", Class="staff-icon",URL = Url.Action("index", "Hostess")},
        //            new MenuModel{Name = "Staff", Class="staff-icon",URL = Url.Action("index", "Staff")},
        //            new MenuModel{Name = "Setting",Class="", URL = Url.Action("index","AccountInfo")}
        //        };

        //        if (usr.RestaurantChainId == 87)//only for cafe 163 
        //        {
        //            menu.Menus[1].URL = Url.Action("table", "Seating");
        //        }
        //    }
        //    else if (role == "user")//hostess/waitstaff
        //    {
        //        if (usr.StaffTypeId == (int)StaffTypeEnum.Hostess)
        //        {
        //            menu.Menus = new List<MenuModel>
        //        {
        //            //new MenuModel{Name = "DASHBOARD", URL = "#"},
        //            new MenuModel{Name = "Wait List",Class="wait-icon", URL = Url.Action("index", "WaitList")},
        //            new MenuModel{Name = "Seating",Class="seat-icon", URL = Url.Action("index", "Seating")},
        //            new MenuModel{Name = "Metrics", Class="metric-icon",URL = Url.Action("index", "Metrics")}
        //            //new MenuModel{Name = "Sign out", URL = Url.Action("logoff","Account", new { area = "" })}
        //        };
        //        }
        //        else
        //        {
        //            menu.Menus = new List<MenuModel>
        //        {
        //            new MenuModel{Name = "Seating",Class="seat-icon", URL = Url.Action("index", "Seating")},
        //            new MenuModel{Name = "Metrics", Class="metric-icon",URL = Url.Action("index", "Metrics")}
        //        };
        //        }
        //    }
        //    return menu;
        //}
    }
}
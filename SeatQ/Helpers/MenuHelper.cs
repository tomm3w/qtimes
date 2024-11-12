using SeatQ.Areas.Admin.Models;
using SeatQ.core.dal.Enums;
using SeatQ.core.dal.Infrastructure.Repositories;
using SeatQ.core.dal.Models;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace SeatQ.Helpers
{
    public class MenuHelper
    {
        public MenuListModel GetMenu()
        {
            UrlHelper Url = new UrlHelper(HttpContext.Current.Request.RequestContext);
            MenuListModel menu = new MenuListModel();
            AccountInfoRepository userRepository = new AccountInfoRepository();
            UserProfile usr = userRepository.GetUserByUserName(Authentication.Web.Security.CurrentUserName);
            string role = (usr.RoleName == null) ? "user" : usr.RoleName.ToLower();

            if (role == "administrator" || role == "regional manager")
            {
                menu.Menus = new List<MenuModel>
                {
                    new MenuModel{Name = "Wait List",Class="seat-icon-grey", URL = Url.Action("index", "WaitList")},
                    new MenuModel{Name = "Seating",Class="seat-icon", URL = Url.Action("index", "Seating")},
                    new MenuModel{Name = "Loyalty",Class="resv-icon grey_i",URL = Url.Action("index", "ReturnGuest")},
                    new MenuModel{Name = "Messages",Class="message-icon", URL = Url.Action("index", "Message")},
                    new MenuModel{Name = "Metrics", Class="metrics-icon",URL = Url.Action("index", "Metrics")},
                    new MenuModel{Name = "Staff", Class="staff-icon",URL = Url.Action("index", "Staff")}
                };

                if (usr.RestaurantChainId == 87)//only for cafe 163 
                {
                    menu.Menus[1].URL = Url.Action("table", "Seating");
                }
            }
            else if (role == "user")//hostess/waitstaff
            {
                if (usr.StaffTypeId == (int)StaffTypeEnum.Hostess)
                {
                    menu.Menus = new List<MenuModel>
                {
                    //new MenuModel{Name = "DASHBOARD", URL = "#"},
                    new MenuModel{Name = "Wait List",Class="wait-icon", URL = Url.Action("index", "WaitList")},
                    new MenuModel{Name = "Seating",Class="seat-icon", URL = Url.Action("index", "Seating")},
                    new MenuModel{Name = "Metrics", Class="metric-icon",URL = Url.Action("index", "Metrics")}
                    //new MenuModel{Name = "Sign out", URL = Url.Action("logoff","Account", new { area = "" })}
                };
                }
                else
                {
                    menu.Menus = new List<MenuModel>
                {
                    new MenuModel{Name = "Seating",Class="seat-icon", URL = Url.Action("index", "Seating")},
                    new MenuModel{Name = "Metrics", Class="metric-icon",URL = Url.Action("index", "Metrics")}
                };
                }
            }
            return menu;
        }
    }
}
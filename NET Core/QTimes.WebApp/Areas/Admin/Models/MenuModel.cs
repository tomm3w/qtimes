using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SeatQ.Areas.Admin.Models
{
    public class MenuListModel
    {
        public MenuModel selectedMenu { get; set; }
        public List<MenuModel> Menus { get; set; }
    }
    public class MenuModel
    {
        public string Name;
        public string Class;
        public string URL;
    }

    public class ContactModel
    {
        [Required]
        [Display(Name = "Your Name")]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [Display(Name = "Business Name")]
        public string BusinessName { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        public string Message { get; set; }
    }
}
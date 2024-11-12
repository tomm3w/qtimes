//using common.api.MultipartDataMediaFormatter;
//using Core.Attributes;
//using SeatQ.core.dal.Enums;
//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
//using System.Web;

//namespace SeatQ.core.dal.Models
//{
//    public class SignupModel
//    {
//        [Required]
//        public string FullName { get; set; }

//        [Required]
//        public string BusinessName { get; set; }

//        [Required]
//        [EmailAddress]
//        public string Email { get; set; }

//        [Required]
//        [Display(Name = "Address")]
//        public string Address1 { get; set; }

//        public string Address2 { get; set; }

//        [Required]
//        public string CityTown { get; set; }

//        [Required]
//        public string State { get; set; }

//        [Required]
//        public string Zip { get; set; }

//        public string Country { get; set; }

//        [Required]
//        public string Phone { get; set; }

//        [Required]
//        public string Username { get; set; }

//        [Required]
//        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
//        [DataType(DataType.Password)]
//        public string Password { get; set; }

//        [DataType(DataType.Password)]
//        [Compare("Password", ErrorMessage = "The password and confirmation password does not match.")]
//        public string ConfirmPassword { get; set; }

//        [Required]
//        [EnforceBool(Value = true, ErrorMessage = "You must accept the terms and conditions.")]
//        public bool TermsAndConditions { get; set; }
//    }

//    public class RestaurantInfoModel
//    {
//        public int RestaurantId { get; set; }

//        public int RestaurantChainId { get; set; }

//        public Guid? UserId { get; set; }

//        [Required]
//        public string FullName { get; set; }

//        [Required]
//        public string BusinessName { get; set; }

//        [Required]
//        [EmailAddress]
//        public string Email { get; set; }

//        [Required]
//        public string Address1 { get; set; }

//        public string Address2 { get; set; }

//        [Required]
//        public string CityTown { get; set; }

//        [Required]
//        public string State { get; set; }

//        [Required]
//        public string Zip { get; set; }

//        public string Country { get; set; }

//        [Required]
//        public string Phone { get; set; }

//        public string RestaurantNumber { get; set; }

//        //public short AverageWaitTime { get; set; }

//        [FileType("png,jpg", ErrorMessage = "Not a Valid Image")]
//        public HttpPostedFileBase Image { get; set; }
//        public string LogoPath { get; set; }
//    }

//    public class AccountInfoModel
//    {
//        [Required]
//        public Guid UserId { get; set; }

//        [Required]
//        public string UserName { get; set; }

//        [Required]
//        [EmailAddress]
//        public string Email { get; set; }

//        [Required]
//        public bool IsActive { get; set; }

//        [DataType(DataType.Password)]
//        public string CurrentPassword { get; set; }

//        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
//        [DataType(DataType.Password)]
//        public string NewPassword { get; set; }

//        [DataType(DataType.Password)]
//        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
//        public string ConfirmNewPassword { get; set; }

//    }

//    public class EmailModel
//    {
//        [Required]
//        [EmailAddress(ErrorMessage = "Invalid email address.")]
//        public string Email { get; set; }
//    }

//    public class HostessModel
//    {

//        public Nullable<int> RestaurantChainId { get; set; }

//        public Guid? UserId { get; set; }

//        [Required]
//        public string UserName { get; set; }

//        [Required]
//        [EmailAddress]
//        public string Email { get; set; }

//        [Required]
//        public bool IsActive { get; set; }

//        public bool IsDeleted { get; set; }

//        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
//        [DataType(DataType.Password)]
//        public string Password { get; set; }
//        public StaffTypeEnum StaffTypeId { get; set; }
//    }

//    public class LogoModel
//    {
//        public HttpFile Image { get; set; }
//        [Required]
//        public Nullable<int> RestaurantId { get; set; }
//    }

//    public class ResetPasswordModel
//    {
//        [Required]
//        [Display(Name = "Username")]
//        public string UserName { get; set; }

//        /*
//        [Required]*/
//        [EmailAddress]
//        public string Email { get; set; }
//    }

//    public class RestaurantBusinessModel
//    {
//        public Guid UserId { get; set; }
//        public int RestaurantChainId { get; set; }
//        [Required]
//        public int BusinessId { get; set; }

//        public string BusinessName { get; set; }
//    }

//    public class PreferencesModel
//    {
//        [Required]
//        public int RestaurantChainId { get; set; }
//        public TimeSpan? OpeningHour { get; set; }
//        public TimeSpan? ClosingHour { get; set; }
//        public int? WaittimeInterval { get; set; }
//        public List<int> ClosedDay { get; set; }
//    }

//    public class RestaurantTableModel
//    {
//        public int? RestaurantChainId { get; set; }
//        public int? TableId { get; set; }
//        [Required]
//        [Display(Name = "Table Number")]
//        public string TableNumber { get; set; }
//        [Required]
//        [Display(Name = "Table Type")]
//        public Nullable<int> TableTypeId { get; set; }
//        public Nullable<bool> IsAvailable { get; set; }
//    }
//}
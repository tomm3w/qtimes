//using iVeew.common.api.Commands;
//using QTimes.core.dal.Enums;
//using System;
//using System.ComponentModel.DataAnnotations;

//namespace QTimes.api.Commands.EditUser
//{
//    public class EditUserRequest : ICommandRequest
//    {
//        [Required]
//        public Guid Id { get; set; }
//        [Required]
//        public string Username { get; set; }
//        public string Email { get; set; }
//        [Required]
//        public Guid RoleId { get; set; }
//        public string Password { get; set; }
//        public string NewPassword { get; set; }
//        public string ConfirmNewPassword { get; set; }
//        public bool IsActive { get; set; }
//        public StaffTypeEnum StaffTypeId { get; set; }
//        public EditUserRequest()
//        {

//        }
//    }
//}
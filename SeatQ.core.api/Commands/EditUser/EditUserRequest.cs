using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using common.api.Commands;
using SeatQ.core.common.Dto;
using SeatQ.core.dal.Enums;

namespace iDestn.core.api.Commands.EditUser
{
    public class EditUserRequest : ICommandRequest
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public string Username { get; set; }
        public string Email { get; set; }
        [Required]
        public Guid RoleId { get; set; }
        public string Password { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmNewPassword { get; set; }
        public bool IsActive { get; set; }
        public StaffTypeEnum StaffTypeId { get; set; }
        public EditUserRequest()
        {

        }
    }
}
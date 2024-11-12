﻿using iVeew.common.api.Commands;
using System;
using System.ComponentModel.DataAnnotations;

namespace QTimes.api.Commands.Reservation.AccountSetup
{
    public class AccountSetupRequest : ICommandRequest
    {
        public Guid UserId { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string CurrentPassword { get; set; }
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password does not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        public string BusinessName { get; set; }
        [Required]
        public string FullName { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string State { get; set; }
        [Required]
        public string Zip { get; set; }
        public string TimezoneOffset { get; set; }
        public string TimezoneOffsetValue { get; set; }
    }
}
using common.api.Commands;
using System;
using System.ComponentModel.DataAnnotations;

namespace SeatQ.core.api.Commands.Reservation.BusinessProfile
{
    public class BusinessProfileRequest : ICommandRequest
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        [Required]
        public string BusinessName { get; set; }
        [Required]
        public string FullName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string LogoPath { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string State { get; set; }
        [Required]
        public string Zip { get; set; }
        [Required]
        public string VirtualNo { get; set; }
        [Required]
        public string PhoneNo { get; set; }
        public string MobileNo { get; set; }
        public string ShortUrl { get; set; }

        public string TimezoneOffset { get; set; }
        public string TimezoneOffsetValue { get; set; }
        public int PassTemplateId { get; set; }
    }
}
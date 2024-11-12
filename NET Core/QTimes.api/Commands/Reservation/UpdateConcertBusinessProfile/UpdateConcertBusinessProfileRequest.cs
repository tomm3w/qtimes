using iVeew.common.api.Commands;
using System;
using System.ComponentModel.DataAnnotations;

namespace QTimes.api.Commands.Reservation.UpdateConcertBusinessProfile
{
    public class UpdateConcertBusinessProfileRequest : ICommandRequest
    {
        public Guid UserId { get; set; }
        public Guid ReservationBusinessId { get; set; }
        [Required]
        public Guid ConcertId { get; set; }
        [Required]
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public Nullable<bool> EnablePrivacyPolicy { get; set; }
        public Nullable<bool> EnableServiceTerms { get; set; }
        public Nullable<bool> EnableCommunityGuidelines { get; set; }
        public string PrivacyPolicyFilePath { get; set; }
        public string ServiceTermsFilePath { get; set; }
        public string CummunityGuidelinesFilePath { get; set; }
        public string PassTemplateId { get; set; }
        [Required]
        public string VirtualNo { get; set; }
        [Required]
        public string PhoneNo { get; set; }
        public string MobileNo { get; set; }
    }
}
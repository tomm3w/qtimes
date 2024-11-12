using iVeew.common.api.Commands;
using System;
using System.ComponentModel.DataAnnotations;

namespace QTimes.api.Commands.Reservation.BusinessProfile
{
    public class BusinessProfileRequest : ICommandRequest
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string LogoPath { get; set; }
        [Required]
        public string VirtualNo { get; set; }
        [Required]
        public string PhoneNo { get; set; }
        public string MobileNo { get; set; }
        public string ShortUrl { get; set; }
        public int PassTemplateId { get; set; }
        [Required]
        public string BusinessDetailName { get; set; }
        [Required]
        public string Description { get; set; }
        public Nullable<bool> EnablePrivacyPolicy { get; set; }
        public Nullable<bool> EnableServiceTerms { get; set; }
        public Nullable<bool> EnableCommunityGuidelines { get; set; }
        public string PrivacyPolicyFilePath { get; set; }
        public string ServiceTermsFilePath { get; set; }
        public string CummunityGuidelinesFilePath { get; set; }
    }
}
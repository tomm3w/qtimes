using System;
using System.Collections.Generic;

namespace QTimes.core.dal.Models
{
    public partial class BusinessDetail
    {
        public BusinessDetail()
        {
            Reservation = new HashSet<Reservation>();
            ReservationGuest = new HashSet<ReservationGuest>();
            ReservationMessageRule = new HashSet<ReservationMessageRule>();
        }

        public Guid Id { get; set; }
        public Guid? ReservationBusinessId { get; set; }
        public short? BusinessTypeId { get; set; }
        public string BusinessName { get; set; }
        public string LogoPath { get; set; }
        public string OpenDayFrom { get; set; }
        public string OpenDayTo { get; set; }
        public TimeSpan? OpenHourFrom { get; set; }
        public TimeSpan? OpenHourTo { get; set; }
        public bool? IsLimitSizePerHour { get; set; }
        public short? LimitSizePerHour { get; set; }
        public bool? IsLimitTimePerReservation { get; set; }
        public short? MinLimitTimePerReservation { get; set; }
        public short? MaxLimitTimePerReservation { get; set; }
        public bool? IsLimitSizePerGroup { get; set; }
        public short? LimitSizePerGroup { get; set; }
        public string ShortUrl { get; set; }
        public bool? IsDlscanEnabled { get; set; }
        public bool? IsChargeEnabled { get; set; }
        public bool? IsSeatmapEnabled { get; set; }
        public short? NoOfSeatRows { get; set; }
        public short? NoOfSeatColumns { get; set; }
        public int? PassTemplateId { get; set; }
        public string VirtualNo { get; set; }
        public string PhoneNo { get; set; }
        public string MobileNo { get; set; }
        public bool? EnablePrivacyPolicy { get; set; }
        public bool? EnableServiceTerms { get; set; }
        public bool? EnableCommunityGuidelines { get; set; }
        public string PrivacyPolicyFilePath { get; set; }
        public string ServiceTermsFilePath { get; set; }
        public string CummunityGuidelinesFilePath { get; set; }
        public string Description { get; set; }
        public DateTime? DeletedDate { get; set; }

        public virtual BusinessType BusinessType { get; set; }
        public virtual ReservationBusiness ReservationBusiness { get; set; }
        public virtual ICollection<Reservation> Reservation { get; set; }
        public virtual ICollection<ReservationGuest> ReservationGuest { get; set; }
        public virtual ICollection<ReservationMessageRule> ReservationMessageRule { get; set; }
    }
}

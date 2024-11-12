using System;
using System.Collections.Generic;

namespace QTimes.core.dal.Models
{
    public partial class ConcertEvent
    {
        public ConcertEvent()
        {
            ConcertEventMessageRule = new HashSet<ConcertEventMessageRule>();
            ConcertEventReservation = new HashSet<ConcertEventReservation>();
            ConcertEventSeatLock = new HashSet<ConcertEventSeatLock>();
            ConcertEventSeating = new HashSet<ConcertEventSeating>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string PrivacyPolicy { get; set; }
        public string ServiceTerms { get; set; }
        public string CummunityGuidelines { get; set; }
        public string ImagePath { get; set; }
        public Guid ConcertId { get; set; }
        public DateTime? Date { get; set; }
        public TimeSpan? TimeFrom { get; set; }
        public TimeSpan? TimeTo { get; set; }
        public bool? EnableTotalCapacity { get; set; }
        public short? TotalCapacity { get; set; }
        public bool? EnableGroupSize { get; set; }
        public short? MinGroupSize { get; set; }
        public short? MaxGroupSize { get; set; }
        public bool? EnableTimeExpiration { get; set; }
        public short? TimeExpiration { get; set; }
        public bool? IsDlscanEnabled { get; set; }
        public bool? IsChargeEnabled { get; set; }
        public bool? IsSeatmapEnabled { get; set; }
        public string SeatMapPath { get; set; }
        public string SeatType { get; set; }
        public bool? AllowCheckInTime { get; set; }
        public int? PassTemplateId { get; set; }
        public string VirtualNo { get; set; }
        public string PhoneNo { get; set; }
        public string MobileNo { get; set; }
        public string ShortUrl { get; set; }
        public bool? EnablePrivacyPolicy { get; set; }
        public bool? EnableServiceTerms { get; set; }
        public bool? EnableCommunityGuidelines { get; set; }
        public string PrivacyPolicyFilePath { get; set; }
        public string ServiceTermsFilePath { get; set; }
        public string CummunityGuidelinesFilePath { get; set; }
        public DateTime? DeletedDate { get; set; }

        public virtual Concert Concert { get; set; }
        public virtual ICollection<ConcertEventMessageRule> ConcertEventMessageRule { get; set; }
        public virtual ICollection<ConcertEventReservation> ConcertEventReservation { get; set; }
        public virtual ICollection<ConcertEventSeatLock> ConcertEventSeatLock { get; set; }
        public virtual ICollection<ConcertEventSeating> ConcertEventSeating { get; set; }
    }
}

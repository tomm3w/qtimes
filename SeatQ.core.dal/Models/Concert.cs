//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SeatQ.core.dal.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Concert
    {
        public Concert()
        {
            this.ConcertMessageRules = new HashSet<ConcertMessageRule>();
            this.ConcertReservations = new HashSet<ConcertReservation>();
            this.ConcertSeatings = new HashSet<ConcertSeating>();
            this.ConcertSeatLocks = new HashSet<ConcertSeatLock>();
        }
    
        public System.Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string PrivacyPolicy { get; set; }
        public string ServiceTerms { get; set; }
        public string CummunityGuidelines { get; set; }
        public string ImagePath { get; set; }
        public System.Guid ReservationBusinessId { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public Nullable<System.TimeSpan> TimeFrom { get; set; }
        public Nullable<System.TimeSpan> TimeTo { get; set; }
        public Nullable<bool> EnablePrivacyPolicy { get; set; }
        public Nullable<bool> EnableServiceTerms { get; set; }
        public Nullable<bool> EnableCommunityGuidelines { get; set; }
        public string PrivacyPolicyFilePath { get; set; }
        public string ServiceTermsFilePath { get; set; }
        public string CummunityGuidelinesFilePath { get; set; }
        public string OpenDayFrom { get; set; }
        public string OpenDayTo { get; set; }
        public Nullable<System.TimeSpan> OpenHourFrom { get; set; }
        public Nullable<System.TimeSpan> OpenHourTo { get; set; }
        public Nullable<bool> EnableTotalCapacity { get; set; }
        public Nullable<short> TotalCapacity { get; set; }
        public Nullable<bool> EnableGroupSize { get; set; }
        public Nullable<short> MinGroupSize { get; set; }
        public Nullable<short> MaxGroupSize { get; set; }
        public Nullable<bool> EnableTimeExpiration { get; set; }
        public Nullable<short> TimeExpiration { get; set; }
        public Nullable<bool> IsDLScanEnabled { get; set; }
        public Nullable<bool> IsChargeEnabled { get; set; }
        public Nullable<bool> IsSeatmapEnabled { get; set; }
        public string SeatMapPath { get; set; }
        public string SeatType { get; set; }
        public Nullable<bool> AllowCheckInTime { get; set; }
        public Nullable<int> PassTemplateId { get; set; }
        public string VirtualNo { get; set; }
        public string PhoneNo { get; set; }
        public string MobileNo { get; set; }
    
        public virtual ReservationBusiness ReservationBusiness { get; set; }
        public virtual ICollection<ConcertMessageRule> ConcertMessageRules { get; set; }
        public virtual ICollection<ConcertReservation> ConcertReservations { get; set; }
        public virtual ICollection<ConcertSeating> ConcertSeatings { get; set; }
        public virtual ICollection<ConcertSeatLock> ConcertSeatLocks { get; set; }
    }
}

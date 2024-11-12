using common.api.Queries;
using System;
using System.Collections.Generic;

namespace SeatQ.core.api.Queries.Reservation.GetConcertById
{
    public class GetConcertByIdResponse : IQueryResponse
    {
        public string UserName { get; set; }
        public string BusinessName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string LogoPath { get; set; }
        public string LogoFullPath { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string TimezoneOffset { get; set; }
        public string TimezoneOffsetValue { get; set; }

        public string VirtualNo { get; set; }
        public string PhoneNo { get; set; }
        public string MobileNo { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string PrivacyPolicy { get; set; }
        public string ServiceTerms { get; set; }
        public string CummunityGuideLines { get; set; }
        public string ImagePath { get; set; }
        public string ImageFullPath { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public Nullable<System.TimeSpan> TimeFrom { get; set; }
        public Nullable<System.TimeSpan> TimeTo { get; set; }
        public Nullable<bool> EnablePrivacyPolicy { get; set; }
        public Nullable<bool> EnableServiceTerms { get; set; }
        public Nullable<bool> EnableCommunityGuidelines { get; set; }
        public string PrivacyPolicyFilePath { get; set; }
        public string PrivacyPolicyFileFullPath { get; set; }
        public string ServiceTermsFilePath { get; set; }
        public string ServiceTermsFileFullPath { get; set; }
        public string CummunityGuidelinesFilePath { get; set; }
        public string CummunityGuidelinesFileFullPath { get; set; }
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
        public string SeatMapFullPath { get; set; }
        public string SeatType { get; set; }
        public Nullable<bool> AllowCheckInTime { get; set; }
        public List<ConcertSpot> ConcertSeatings { get; set; }
        public string PassTemplateId { get; set; }
    }

    public class ConcertSpot
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public Nullable<short> Spots { get; set; }
        public Nullable<short> SeatsPerSpot { get; set; }
        public Nullable<System.TimeSpan> CheckInTime { get; set; }
    }
}
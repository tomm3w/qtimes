using common.api.Queries;
using System;

namespace SeatQ.core.api.Queries.Reservation.GetBusinessProfile
{
    public class GetBusinessProfileResponse : IQueryResponse
    {
        public System.Guid Id { get; set; }
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
        public string VirtualNo { get; set; }
        public string PhoneNo { get; set; }
        public string MobileNo { get; set; }
        public string OpenDayFrom { get; set; }
        public string OpenDayTo { get; set; }
        public Nullable<System.TimeSpan> OpenHourFrom { get; set; }
        public Nullable<System.TimeSpan> OpenHourTo { get; set; }
        public Nullable<bool> IsLimitSizePerHour { get; set; }
        public Nullable<short> LimitSizePerHour { get; set; }
        public Nullable<bool> IsLimitTimePerReservation { get; set; }
        public Nullable<short> MinLimitTimePerReservation { get; set; }
        public Nullable<short> MaxLimitTimePerReservation { get; set; }
        public Nullable<bool> IsLimitSizePerGroup { get; set; }
        public Nullable<short> LimitSizePerGroup { get; set; }
        public string ShortUrl { get; set; }
        public string TimezoneOffset { get; set; }
        public string TimezoneOffsetValue { get; set; }
        public Nullable<bool> IsDLScanEnabled { get; set; }
        public Nullable<bool> IsChargeEnabled { get; set; }
        public Nullable<bool> IsSeatmapEnabled { get; set; }
        public Nullable<short> NoOfSeatRows { get; set; }
        public Nullable<short> NoOfSeatColumns { get; set; }
        public int PassTemplateId { get; set; }
    }
}
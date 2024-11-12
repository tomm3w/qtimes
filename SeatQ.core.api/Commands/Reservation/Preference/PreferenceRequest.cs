using common.api.Commands;
using System;
using System.ComponentModel.DataAnnotations;

namespace SeatQ.core.api.Commands.Reservation.Preference
{
    public class PreferenceRequest : ICommandRequest
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        [Required]
        public string OpenDayFrom { get; set; }
        [Required]
        public string OpenDayTo { get; set; }
        [Required]
        public Nullable<System.TimeSpan> OpenHourFrom { get; set; }
        [Required]
        public Nullable<System.TimeSpan> OpenHourTo { get; set; }
        [Required]
        public Nullable<bool> IsLimitSizePerHour { get; set; }
        [Required]
        public Nullable<short> LimitSizePerHour { get; set; }
        [Required]
        public Nullable<bool> IsLimitTimePerReservation { get; set; }
        [Required]
        public Nullable<short> MinLimitTimePerReservation { get; set; }
        [Required]
        public Nullable<short> MaxLimitTimePerReservation { get; set; }
        [Required]
        public Nullable<bool> IsLimitSizePerGroup { get; set; }
        [Required]
        public Nullable<short> LimitSizePerGroup { get; set; }
        public Nullable<bool> IsDLScanEnabled { get; set; }
        public Nullable<bool> IsChargeEnabled { get; set; }
        public Nullable<bool> IsSeatmapEnabled { get; set; }
        public Nullable<short> NoOfSeatRows { get; set; }
        public Nullable<short> NoOfSeatColumns { get; set; }
    }
}
using common.api.Commands;
using System;
using System.ComponentModel.DataAnnotations;

namespace SeatQ.core.api.Commands.Reservation.UpdateConcertPreference
{
    public class UpdateConcertPreferenceRequest : ICommandRequest
    {
        public Guid UserId { get; set; }
        public Guid ReservationBusinessId { get; set; }
        [Required]
        public Guid ConcertId { get; set; }
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
    }
}
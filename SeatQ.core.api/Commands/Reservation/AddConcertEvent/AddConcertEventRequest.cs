using common.api.Commands;
using System;
using System.ComponentModel.DataAnnotations;

namespace SeatQ.core.api.Commands.Reservation.AddConcertEvent
{
    public class AddConcertEventRequest : ICommandRequest
    {
        public Guid UserId { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public string PrivacyPolicy { get; set; }
        public string ServiceTerms { get; set; }
        public string CummunityGuideLines { get; set; }
        public string ImagePath { get; set; }
        public System.Guid ReservationBusinessId { get; set; }
        [Required]
        public Nullable<System.DateTime> Date { get; set; }
        [Required]
        public Nullable<System.TimeSpan> TimeFrom { get; set; }
        [Required]
        public Nullable<System.TimeSpan> TimeTo { get; set; }
    }
}
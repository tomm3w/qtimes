using iVeew.common.api.Commands;
using System;
using System.ComponentModel.DataAnnotations;

namespace QTimes.api.Commands.Reservation.UpdateConcertEvent
{
    public class UpdateConcertEventRequest : ICommandRequest
    {
        public Guid UserId { get; set; }
        [Required]
        public System.Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public System.Guid ConcertId { get; set; }
        [Required]
        public Nullable<System.DateTime> Date { get; set; }
        [Required]
        public Nullable<System.TimeSpan> TimeFrom { get; set; }
        [Required]
        public Nullable<System.TimeSpan> TimeTo { get; set; }
    }
}
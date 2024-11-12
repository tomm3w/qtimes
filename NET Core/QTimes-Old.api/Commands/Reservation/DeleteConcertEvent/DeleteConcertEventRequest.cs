using iVeew.common.api.Commands;
using System;
using System.ComponentModel.DataAnnotations;

namespace QTimes.api.Commands.Reservation.DeleteConcertEvent
{
    public class DeleteConcertEventRequest : ICommandRequest
    {
        public Guid UserId { get; set; }
        [Required]
        public System.Guid Id { get; set; }
        [Required]
        public System.Guid ConcertId { get; set; }
    }
}
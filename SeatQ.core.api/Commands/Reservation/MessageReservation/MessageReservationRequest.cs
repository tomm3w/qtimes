using common.api.Commands;
using System;
using System.ComponentModel.DataAnnotations;

namespace SeatQ.core.api.Commands.Reservation.MessageReservation
{
    public class MessageReservationRequest : ICommandRequest
    {
        [Required]
        public int? Id { get; set; }
        [Required]
        public string MessageSent { get; set; }
        public Guid MessageSentBy { get; set; }
    }
}
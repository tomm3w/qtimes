using iVeew.common.api.Commands;
using System;
using System.ComponentModel.DataAnnotations;

namespace QTimes.api.Commands.Reservation.MessageConcertEventReservation
{
    public class MessageConcertEventReservationRequest : ICommandRequest
    {
        [Required]
        public int? Id { get; set; }
        [Required]
        public string MessageSent { get; set; }
        public Guid MessageSentBy { get; set; }
    }
}
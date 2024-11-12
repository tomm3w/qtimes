using common.api.Commands;
using System;
using System.ComponentModel.DataAnnotations;

namespace SeatQ.core.api.Commands.Reservation.UpdateReservationMessageRule
{
    public class UpdateConcertMessageRuleRequest : ICommandRequest
    {
        [Required]
        public int Id { get; set; }
        public Nullable<System.Guid> ConcertId { get; set; }
        [Required]
        public string MessageType { get; set; }
        [Required]
        public Nullable<short> Value { get; set; }
        [Required]
        public string ValueType { get; set; }
        [Required]
        public string BeforeAfter { get; set; }
        [Required]
        public string InOut { get; set; }
        [Required]
        public string Message { get; set; }
        public Guid UserId { get; set; }
    }
}
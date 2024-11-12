using common.api.Commands;
using System;
using System.ComponentModel.DataAnnotations;

namespace SeatQ.core.api.Commands.Reservation.AddConcertMessageRule
{
    public class AddConcertMessageRuleRequest : ICommandRequest
    {
        [Required]
        public Nullable<System.Guid> ConcertId { get; set; }
        [Required]
        public string MessageType { get; set; }
        [Required]
        public Nullable<short> Value { get; set; }
        public string ValueType { get; set; }
        public string BeforeAfter { get; set; }
        public string InOut { get; set; }
        [Required]
        public string Message { get; set; }
        public Guid UserId { get; set; }
    }
}
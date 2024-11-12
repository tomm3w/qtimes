using iVeew.common.api.Commands;
using System;
using System.ComponentModel.DataAnnotations;

namespace QTimes.api.Commands.Reservation.UpdateConcertEventMessageRule
{
    public class UpdateConcertEventMessageRuleRequest : ICommandRequest
    {
        [Required]
        public int Id { get; set; }
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
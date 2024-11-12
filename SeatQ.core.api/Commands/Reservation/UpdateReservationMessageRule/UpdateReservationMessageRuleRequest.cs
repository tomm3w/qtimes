using common.api.Commands;
using System;
using System.ComponentModel.DataAnnotations;

namespace SeatQ.core.api.Commands.Reservation.UpdateReservationMessageRule
{
    public class UpdateReservationMessageRuleRequest : ICommandRequest
    {
        public int Id { get; set; }
        public Nullable<System.Guid> BusinessDetailId { get; set; }
        public string MessageType { get; set; }
        public Nullable<short> Value { get; set; }
        public string ValueType { get; set; }
        public string BeforeAfter { get; set; }
        public string InOut { get; set; }
        public string Message { get; set; }
        public Guid UserId { get; set; }
    }
}
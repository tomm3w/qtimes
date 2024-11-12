using iVeew.common.api.Commands;
using System;

namespace QTimes.api.Commands.Reservation.DeleteConcertEventMessageRule
{
    public class DeleteConcertEventMessageRuleRequest : ICommandRequest
    {
        public int Id { get; set; }
        public Nullable<System.Guid> ConcertEventId { get; set; }
        public Guid UserId { get; set; }
    }
}
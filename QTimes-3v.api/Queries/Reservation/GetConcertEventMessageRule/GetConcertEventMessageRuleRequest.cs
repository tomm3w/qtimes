using iVeew.common.api.Queries;
using System;
using System.ComponentModel.DataAnnotations;

namespace QTimes.api.Queries.Reservation.GetConcertEventMessageRule
{
    public class GetConcertEventMessageRuleRequest : IQueryRequest
    {
        public Guid UserId { get; set; }
        [Required]
        public Guid? ConcertEventId { get; set; }
    }
}
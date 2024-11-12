using common.api.Queries;
using System;
using System.ComponentModel.DataAnnotations;

namespace SeatQ.core.api.Queries.Reservation.GetConcertMessageRule
{
    public class GetConcertMessageRuleRequest : IQueryRequest
    {
        public Guid UserId { get; set; }
        [Required]
        public Nullable<System.Guid> ConcertId { get; set; }
    }
}
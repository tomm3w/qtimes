using common.api.Queries;
using System;

namespace SeatQ.core.api.Queries.Reservation.GetReservationMessageRule
{
    public class GetReservationMessageRuleRequest : IQueryRequest
    {
        public Guid UserId { get; set; }
        public Guid BusinessDetailId { get; set; }
    }
}
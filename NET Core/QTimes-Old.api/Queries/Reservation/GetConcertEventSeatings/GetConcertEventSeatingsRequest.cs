using iVeew.common.api.Queries;
using System;

namespace QTimes.api.Queries.Reservation.GetConcertEventSeatings
{
    public class GetConcertEventSeatingsRequest : IQueryRequest
    {
        public Guid UserId { get; set; }
        public Guid ConcertEventId { get; set; }
    }
}
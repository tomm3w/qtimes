using common.api.Queries;
using System;

namespace SeatQ.core.api.Queries.Reservation.GetConcertSeatings
{
    public class GetConcertSeatingsRequest : IQueryRequest
    {
        public Guid UserId { get; set; }
        public Guid ConcertId { get; set; }
    }
}
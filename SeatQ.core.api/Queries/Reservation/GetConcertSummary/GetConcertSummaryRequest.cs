using common.api.Queries;
using System;

namespace SeatQ.core.api.Queries.Reservation.GetConcertSummary
{
    public class GetConcertSummaryRequest : IQueryRequest
    {
        public Guid UserId { get; set; }
        public Guid ConcertId { get; set; }
    }
}
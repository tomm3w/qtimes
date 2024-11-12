using iVeew.common.api.Queries;
using System;

namespace QTimes.api.Queries.Reservation.GetConcertEventSummary
{
    public class GetConcertEventSummaryRequest : IQueryRequest
    {
        public Guid UserId { get; set; }
        public Guid? EventId { get; set; }
        public Guid ConcertId { get; set; }
    }
}
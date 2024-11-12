using iVeew.common.api.Queries;
using System;

namespace QTimes.api.Queries.Reservation.GetConcertMetrics
{
    public class GetConcertMetricsRequest : IQueryRequest
    {
        public Guid UserId { get; set; }
        public DateTime? Date { get; set; }
        public Guid ConcertId { get; set; }
        public Guid? ConcertEventId { get; internal set; }
    }
}
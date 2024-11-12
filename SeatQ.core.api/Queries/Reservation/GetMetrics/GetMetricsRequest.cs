using common.api.Queries;
using System;

namespace QTimes.api.Queries.Reservation.GetMetrics
{
    public class GetMetricsRequest : IQueryRequest
    {
        public Guid UserId { get; set; }
        public DateTime? Date { get; set; }
        public Guid? BusinessDetailId { get; set; }
    }
}
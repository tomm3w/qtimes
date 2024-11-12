using iVeew.common.api.Queries;
using System;

namespace QTimes.api.Queries.Reservation.GetMetrics
{
    public class GetMetricsRequest : IQueryRequest
    {
        public Guid UserId { get; set; }
        public Guid? BusinessDetailId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
using iVeew.common.api.Queries;
using System;

namespace QTimes.api.Queries.Reservation.GetSummary
{
    public class GetSummaryRequest : IQueryRequest
    {
        public Guid UserId { get; set; }
        public DateTime? Date { get; set; }
        public Guid BusinessDetailId { get; set; }
        public TimeSpan? CurrentTime { get; set; }
    }
}
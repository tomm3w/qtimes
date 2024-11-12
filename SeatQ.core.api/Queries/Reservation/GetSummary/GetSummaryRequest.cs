using common.api.Queries;
using System;

namespace SeatQ.core.api.Queries.Reservation.GetSummary
{
    public class GetSummaryRequest : IQueryRequest
    {
        public Guid UserId { get; set; }
        public DateTime? Date { get; set; }
        public Guid BusinessDetailId { get; set; }
    }
}
using common.api.Queries;

namespace SeatQ.core.api.Queries.Reservation.GetMetrics
{
    public class GetMetricsResponse : IQueryResponse
    {
        public double? AvgWaitTime { get; set; }
        public int NoOfPlace { get; set; }
        public int Cancelled { get; set; }
        public double? AvgGroupSize { get; set; }
        public int NewGuest { get; set; }
    }
}
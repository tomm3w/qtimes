using iVeew.common.api.Queries;

namespace QTimes.api.Queries.Reservation.GetConcertMetrics
{
    public class GetConcertMetricsResponse : IQueryResponse
    {
        public int TotalGuests { get; set; }
        public int TotalEvents { get; set; }
        public int TotalReservations { get; set; }
    }
}
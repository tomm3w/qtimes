using common.api.Queries;

namespace SeatQ.core.api.Queries.Reservation.GetConcertMetrics
{
    public class GetConcertMetricsResponse : IQueryResponse
    {
        public int TotalGuests { get; set; }
        public int TotalEvents { get; set; }
        public int TotalReservations { get; set; }
    }
}
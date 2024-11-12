using common.api.Queries;

namespace SeatQ.core.api.Queries.Reservation.GetConcertSummary
{
    public class GetConcertSummaryResponse : IQueryResponse
    {
        public string BusinessName { get; set; }
        public int TotalSpots { get; set; }
        public int Available { get; set; }
        public int Waiting { get; set; }
        public int InPlace { get; set; }
    }
}
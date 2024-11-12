using common.api.Queries;

namespace SeatQ.core.api.Queries.Reservation.GetSummary
{
    public class GetSummaryResponse : IQueryResponse
    {
        public string BusinessName { get; set; }
        public int LimitSize { get; set; }
        public int TotalSpots { get; set; }
        public int Available { get; set; }
        public int Waiting { get; set; }
        public int InPlace { get; set; }
    }
}
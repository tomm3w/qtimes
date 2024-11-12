using iVeew.common.api.Queries;

namespace QTimes.api.Queries.Reservation.GetConcertEventSummary
{
    public class GetConcertEventSummaryResponse : IQueryResponse
    {
        public string BusinessName { get; set; }
        public int TotalSpots { get; set; }
        public int Available { get; set; }
        public int Waiting { get; set; }
        public int InPlace { get; set; }
        public bool EnableGroupSize { get; set; }
        public int MaxGroupSize { get; set; }
        public int ConcertEventSeatingCount { get; set; }
    }
}
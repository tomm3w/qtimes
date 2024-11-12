using iVeew.common.api.Queries;

namespace QTimes.api.Queries.Reservation.GetSummary
{
    public class GetSummaryResponse : IQueryResponse
    {
        public string BusinessName { get; set; }
        public int LimitSize { get; set; }
        public int TotalSpots { get; set; }
        public int Available { get; set; }
        public int Waiting { get; set; }
        public int InPlace { get; set; }
        public bool EnableGroupSize { get; set; }
        public int MaxGroupSize { get; set; }
    }
}
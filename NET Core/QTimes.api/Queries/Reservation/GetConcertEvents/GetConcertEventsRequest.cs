using iVeew.common.api.Queries;
using System;

namespace QTimes.api.Queries.Reservation.GetConcertEvents
{
    public class GetConcertEventsRequest : IQueryRequest
    {
        public Guid UserId { get; set; }
        public DateTime? EventDate { get; set; }
        public string SearchText { get; set; }
        public Guid ConcertId { get; set; }
        public string FilterBy { get; set; }
    }
}
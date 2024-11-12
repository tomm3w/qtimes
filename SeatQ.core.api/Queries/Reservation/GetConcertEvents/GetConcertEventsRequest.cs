using common.api.Queries;
using System;

namespace SeatQ.core.api.Queries.Reservation.GetConcertEvents
{
    public class GetConcertEventsRequest : IQueryRequest
    {
        public Guid UserId { get; set; }
        public Guid ReservationBusinessId { get; set; }
        public DateTime? EventDate { get; set; }
        public string SearchText { get; set; }
    }
}
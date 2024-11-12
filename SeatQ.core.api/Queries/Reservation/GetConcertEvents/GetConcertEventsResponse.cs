using common.api.Queries;
using System;
using System.Collections.Generic;

namespace SeatQ.core.api.Queries.Reservation.GetConcertEvents
{
    public class GetConcertEventsResponse : IQueryResponse
    {
        public List<ConcertEvent> Model { get; set; }
    }

    public class ConcertEvent
    {
        public Guid UserId { get; set; }
        public System.Guid Id { get; set; }
        public string Name { get; set; }
        public System.Guid ReservationBusinessId { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public Nullable<System.TimeSpan> TimeFrom { get; set; }
        public Nullable<System.TimeSpan> TimeTo { get; set; }
        public string EventDate
        {
            get
            {
                return Date.Value.ToString("yyyy-MM-dd");
            }
        }
    }
}
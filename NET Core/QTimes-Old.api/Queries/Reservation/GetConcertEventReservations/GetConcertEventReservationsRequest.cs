using iVeew.common.api.Queries;
using System;

namespace QTimes.api.Queries.Reservation.GetConcertEventReservations
{
    public class GetConcertEventReservationsRequest : IQueryRequest
    {
        public Guid? ConcertEventId { get; set; }
        public int? Page { get; set; }
        public int? PageSize { get; set; }
        public Guid UserId { get; set; }
        public Nullable<System.Guid> ReservationBusinessId { get; set; }
        public DateTime? ReservationDate { get; set; }
        public string SearchText { get; set; }
        public Guid ConcertId { get; set; }
    }
}
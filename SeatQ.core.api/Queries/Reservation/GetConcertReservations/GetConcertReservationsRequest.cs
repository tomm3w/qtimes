using common.api.Queries;
using System;
using System.ComponentModel.DataAnnotations;

namespace SeatQ.core.api.Queries.Reservation.GetConcertReservations
{
    public class GetConcertReservationsRequest : IQueryRequest
    {
        public Guid ConcertId { get; set; }
        public int? Page { get; set; }
        public int? PageSize { get; set; }
        public Guid UserId { get; set; }
        public Nullable<System.Guid> ReservationBusinessId { get; set; }
        public DateTime? ReservationDate { get; set; }
        public string SearchText { get; set; }
    }
}
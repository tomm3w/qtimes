using common.api.Queries;
using System;

namespace SeatQ.core.api.Queries.Reservation.GetReservations
{
    public class GetReservationsRequest : IQueryRequest
    {
        public int? Page { get; set; }
        public int? PageSize { get; set; }
        public Guid UserId { get; set; }
        public Nullable<System.Guid> BusinessDetailId { get; set; }
        public DateTime? ReservationDate { get; set; }
        public string SearchText { get; set; }
    }
}
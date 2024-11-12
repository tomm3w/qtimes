using iVeew.common.api.Queries;
using System;

namespace QTimes.api.Queries.Reservation.GetReservationBusinessById
{
    public class GetReservationBusinessByIdRequest : IQueryRequest
    {
        public Guid Id { get; set; }
        public DateTime? Date { get; set; }
    }
}
using common.api.Queries;
using System;

namespace SeatQ.core.api.Queries.Reservation.GetConcertById
{
    public class GetConcertByIdRequest : IQueryRequest
    {
        public string UserName { get; set; }
        public Guid UserId { get; set; }
        public Guid Id { get; set; }
    }
}
using common.api.Queries;
using System;

namespace SeatQ.core.api.Queries.Reservation.GetConcertMessages
{
    public class GetConcertMessagesRequest : IQueryRequest
    {
        public Guid UserId { get; set; }
        public int ConcertReservationId { get; set; }
        
    }
}
using common.api.Queries;
using System;

namespace SeatQ.core.api.Queries.Reservation.GetMessages
{
    public class GetMessagesRequest : IQueryRequest
    {
        public Guid UserId { get; set; }
        public int ReservationId { get; set; }
        
    }
}
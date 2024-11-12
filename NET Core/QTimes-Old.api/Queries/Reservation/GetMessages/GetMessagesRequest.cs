using iVeew.common.api.Queries;
using System;

namespace QTimes.api.Queries.Reservation.GetMessages
{
    public class GetMessagesRequest : IQueryRequest
    {
        public Guid UserId { get; set; }
        public int ReservationId { get; set; }
        
    }
}
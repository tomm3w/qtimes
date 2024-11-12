using iVeew.common.api.Queries;
using System;

namespace QTimes.api.Queries.Reservation.GetConcertEventMessages
{
    public class GetConcertEventMessagesRequest : IQueryRequest
    {
        public Guid UserId { get; set; }
        public int ConcertEventReseravationId { get; set; }
        
    }
}
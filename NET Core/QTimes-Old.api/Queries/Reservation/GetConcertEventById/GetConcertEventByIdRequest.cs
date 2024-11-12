using iVeew.common.api.Queries;
using System;

namespace QTimes.api.Queries.Reservation.GetConcertEventById
{
    public class GetConcertEventByIdRequest : IQueryRequest
    { 
        public Guid Id { get; set; }
    }
}
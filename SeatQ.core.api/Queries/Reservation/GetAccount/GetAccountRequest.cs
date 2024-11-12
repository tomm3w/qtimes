using common.api.Queries;
using System;

namespace SeatQ.core.api.Queries.Reservation.GetAccount
{
    public class GetAccountRequest : IQueryRequest
    {
        public Guid UserId { get; set; }
    }
}
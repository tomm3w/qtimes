using common.api.Queries;
using System;

namespace SeatQ.core.api.Queries.Reservation.GetBusinessAccounts
{
    public class GetBusinessAccountsRequest : IQueryRequest
    {
        public Guid UserId { get; set; }
    }
}
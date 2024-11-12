using iVeew.common.api.Queries;
using System;

namespace QTimes.api.Queries.Reservation.GetBusinessAccounts
{
    public class GetBusinessAccountsRequest : IQueryRequest
    {
        public Guid UserId { get; set; }
    }
}
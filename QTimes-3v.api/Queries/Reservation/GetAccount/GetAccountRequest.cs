using iVeew.common.api.Queries;
using System;

namespace QTimes.api.Queries.Reservation.GetAccount
{
    public class GetAccountRequest : IQueryRequest
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
    }
}
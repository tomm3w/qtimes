using common.api.Queries;
using System;

namespace SeatQ.core.api.Queries.GetUserById
{
    public class GetUserByIdRequest : IQueryRequest
    {
        public GetUserByIdRequest(Guid userid)
        {
            UserId = userid;
        }

        public Guid UserId { get; private set; }
    }
}
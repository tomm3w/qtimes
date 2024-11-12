using common.api.Queries;
using System.Collections.Generic;
using SeatQ.core.common.Dto;
using SeatQ.core.dal.Models;


namespace SeatQ.core.api.Queries.GetHostess
{
    public class GetHostessResponse : IQueryResponse
    {
        public List<UserProfile> Users { get; private set; }

        public GetHostessResponse(List<UserProfile> users)
        {
            Users = users;
        }
    }
}
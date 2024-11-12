using common.api.Queries;
using System.Collections.Generic;
using SeatQ.core.common.Dto;


namespace SeatQ.core.api.Queries.GetUsers
{
    public class GetUsersResponse : IQueryResponse
    {
        public IEnumerable<UserDto> Users { get; private set; }

        public GetUsersResponse(IEnumerable<UserDto> users)
        {
            Users = users;
        }
    }
}
using common.api.Queries;
using SeatQ.core.common.Dto;

namespace SeatQ.core.api.Queries.GetUserByName
{
    public class GetUserByNameResponse : IQueryResponse
    {
        public UserDto User { get; private set; }

        public GetUserByNameResponse(UserDto user)
        {
            User = user;
        }
    }
}
using common.api.Queries;
using SeatQ.core.common.Dto;
using SeatQ.core.dal.Models;

namespace SeatQ.core.api.Queries.GetUserById
{
    public class GetUserByIdResponse : IQueryResponse
    {
        public UserProfile User { get; private set; }

        public GetUserByIdResponse(UserProfile user)
        {
            User = user;
        }
    }
}
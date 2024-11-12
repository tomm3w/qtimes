using common.api.Queries;

namespace SeatQ.core.api.Queries.GetUserByName
{
    public class GetUserByNameRequest : IQueryRequest
    {
        public GetUserByNameRequest(string username)
        {
            Username = username;
        }

        public string Username { get; private set; }
    }
}
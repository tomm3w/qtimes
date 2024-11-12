using common.api.Queries;

namespace SeatQ.core.api.Queries.Reservation.GetAccount
{
    public class GetAccountResponse : IQueryResponse
    {
        public string UserName { get; set; }
        public string Email { get; set; }
    }
}
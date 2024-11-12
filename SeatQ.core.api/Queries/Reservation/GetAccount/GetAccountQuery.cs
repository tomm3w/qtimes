using common.api.Queries;
using System;
using System.Web.Security;

namespace SeatQ.core.api.Queries.Reservation.GetAccount
{
    public class GetAccountQuery : IQuery<GetAccountResponse, GetAccountRequest>
    {
        public GetAccountQuery()
        {
        }

        public GetAccountResponse Handle(GetAccountRequest request)
        {
            var user = Membership.GetUser(request.UserId);
            if (user == null)
                throw new Exception("User not found.");

            return new GetAccountResponse
            {
                UserName = user.UserName,
                Email = user.Email
            };
        }
    }
}
using common.api.Queries;
using SeatQ.core.dal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SeatQ.core.api.Queries.GetGuestMessages
{
    public class GetGuestMessageByIdResponse : IQueryResponse
    {
        public List<GuestMessage> GuestMessage { get; private set; }
        public GetGuestMessageByIdResponse(List<GuestMessage> guestMessage)
		{
            GuestMessage = guestMessage;
		}
    }
}
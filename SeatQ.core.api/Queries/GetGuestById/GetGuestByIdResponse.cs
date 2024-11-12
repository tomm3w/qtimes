using common.api.Queries;
using SeatQ.core.dal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SeatQ.core.api.Queries.GetGuestById
{
    public class GetGuestByIdResponse : IQueryResponse
    {
        public GuestInfo GuestInfo { get; private set; }
        public GetGuestByIdResponse(GuestInfo guestInfo)
		{
            GuestInfo = guestInfo;
		}
    }
}
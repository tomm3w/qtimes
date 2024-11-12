using common.api.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SeatQ.core.api.Queries.GetGuestById
{
    public class GetGuestByIdRequest : IQueryRequest
    {
        public GetGuestByIdRequest(long? guestId)
		{
            GuestId = guestId;
		}

        public long? GuestId { get; private set; }
    }
}
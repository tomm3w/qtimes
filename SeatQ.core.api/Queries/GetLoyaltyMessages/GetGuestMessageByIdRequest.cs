using common.api.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SeatQ.core.api.Queries.GetLoyaltyMessages
{
    public class GetLoyaltyMessageByIdRequest : IQueryRequest
    {
        public GetLoyaltyMessageByIdRequest(long? guestId)
		{
            GuestId = guestId;
		}

		public long? GuestId { get; private set; }
    }
}
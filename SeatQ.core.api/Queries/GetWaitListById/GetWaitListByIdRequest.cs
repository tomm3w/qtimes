using common.api.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SeatQ.core.api.Queries.GetWaitListById
{
    public class GetWaitListByIdRequest : IQueryRequest
    {
        public GetWaitListByIdRequest(long? waitListId)
		{
            WaitListId = waitListId;
		}

		public long? WaitListId { get; private set; }
    }
}
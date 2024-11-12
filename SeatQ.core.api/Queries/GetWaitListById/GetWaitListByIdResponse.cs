using common.api.Queries;
using SeatQ.core.dal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SeatQ.core.api.Queries.GetWaitListById
{
    public class GetWaitListByIdResponse : IQueryResponse
    {
        public WaitList WaitList { get; private set; }
        public GetWaitListByIdResponse(WaitList waitList)
		{
            WaitList = waitList;
		}
    }
}
using common.api.Queries;
using SeatQ.core.dal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SeatQ.core.api.Queries.GetReadyMessages
{
    public class GetReadyMessagesResponse : IQueryResponse
    {
        public List<ReadyMessage> Model { get; private set; }

        public GetReadyMessagesResponse(List<ReadyMessage> model)
		{
            Model = model;
		}
    }
}
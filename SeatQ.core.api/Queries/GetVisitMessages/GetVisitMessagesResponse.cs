using common.api.Queries;
using SeatQ.core.dal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SeatQ.core.api.Queries.GetVisitMessages
{
    public class GetVisitMessagesResponse : IQueryResponse
    {
        public List<VisitMessage> Model { get; private set; }

        public GetVisitMessagesResponse(List<VisitMessage> model)
		{
            Model = model;
		}
    }
}
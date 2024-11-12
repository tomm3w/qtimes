using common.api.Queries;
using SeatQ.core.dal.Models;
using System.Collections.Generic;

namespace SeatQ.core.api.Queries.GetMessages
{
    public class GetMessagesResponse : IQueryResponse
    {
        public ReadyMessage readyModel { get; private set; }
        public List<VisitMessage> visitModel { get; private set; }

        public GetMessagesResponse(ReadyMessage rModel, List<VisitMessage> vModel)
        {
            readyModel = rModel;
            visitModel = vModel;
        }
    }
}
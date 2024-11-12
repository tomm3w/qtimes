using common.api.Queries;
using common.dal;
using SeatQ.core.dal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SeatQ.core.api.Queries.GetVisitMessages
{
    public class GetVisitMessagesQuery : IQuery<GetVisitMessagesResponse, GetVisitMessagesRequest>
    {
        private readonly IGenericRepository<SeatQEntities, VisitMessage> _visitMsgRepository;
        public GetVisitMessagesQuery(IGenericRepository<SeatQEntities, VisitMessage> visitMsgRepository)
        {
            _visitMsgRepository = visitMsgRepository;
        }
        public GetVisitMessagesResponse Handle(GetVisitMessagesRequest request)
        {
            var users = _visitMsgRepository.FindBy(x => x.RestaurantChainId == request.RestaurantChainId && x.IsDeleted == false).ToList();
            if (users != null)
            {
                return new GetVisitMessagesResponse(users);
            }

            return null;

        }
    }
}
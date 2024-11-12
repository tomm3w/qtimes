using common.api.Queries;
using common.dal;
using SeatQ.core.dal.Models;
using System.Linq;

namespace SeatQ.core.api.Queries.GetMessages
{
    public class GetMessagesQuery : IQuery<GetMessagesResponse, GetMessagesRequest>
    {
        private readonly IGenericRepository<SeatQEntities, ReadyMessage> _readyMsgRepository;
        private readonly IGenericRepository<SeatQEntities, VisitMessage> _visitMsgRepository;
        public GetMessagesQuery(IGenericRepository<SeatQEntities, ReadyMessage> readyMsgRepository, IGenericRepository<SeatQEntities, VisitMessage> visitMsgRepository)
        {
            _readyMsgRepository = readyMsgRepository;
            _visitMsgRepository = visitMsgRepository;
        }

        public GetMessagesResponse Handle(GetMessagesRequest request)
        {
            var rMsg = _readyMsgRepository.FindBy(x => x.RestaurantChainId == request.RestaurantChainId && x.IsDeleted == false).FirstOrDefault();
            var vMsg = _visitMsgRepository.FindBy(x => x.RestaurantChainId == request.RestaurantChainId && x.IsDeleted == false).ToList();
            return new GetMessagesResponse(rMsg, vMsg);
        }
    }
}
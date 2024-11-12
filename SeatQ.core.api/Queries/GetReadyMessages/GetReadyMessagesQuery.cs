using common.api.Queries;
using common.dal;
using SeatQ.core.dal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SeatQ.core.api.Queries.GetReadyMessages
{
    public class GetReadyMessagesQuery : IQuery<GetReadyMessagesResponse, GetReadyMessagesRequest>
    {
        private readonly IGenericRepository<SeatQEntities, ReadyMessage> _readyMsgRepository;
        public GetReadyMessagesQuery(IGenericRepository<SeatQEntities, ReadyMessage> readyMsgRepository)
        {
            _readyMsgRepository = readyMsgRepository;
        }
        public GetReadyMessagesResponse Handle(GetReadyMessagesRequest request)
        {
            var msg = _readyMsgRepository.FindBy(x => x.RestaurantChainId == request.RestaurantChainId && x.IsDeleted == false).ToList();

            if (msg.Count == 0 && request.RestaurantChainId != 0)
            {
                var rmsg = new ReadyMessage();
                //insert default message
                rmsg.RestaurantChainId = request.RestaurantChainId;
                rmsg.ReadyMessage1 = "Your table is ready please check with the hostess.";
                rmsg.IsEnabled = true;
                rmsg.IsDeleted = false;
                rmsg.ModifiedDate = DateTime.UtcNow;
                _readyMsgRepository.Add(rmsg);
                _readyMsgRepository.Save();
            }

            msg = _readyMsgRepository.FindBy(x => x.RestaurantChainId == request.RestaurantChainId).ToList();
            return new GetReadyMessagesResponse(msg);


        }
    }
}
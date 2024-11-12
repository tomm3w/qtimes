using common.api.Commands;
using common.dal;
using SeatQ.core.dal.Models;
using System;
using System.Linq;

namespace SeatQ.core.api.Commands.EditReadyMessage
{
    public class EditReadyMessageCommand : ICommand<EditReadyMessageRequest>
    {

        private readonly IGenericRepository<SeatQEntities, ReadyMessage> _rMsgRepository;
        public EditReadyMessageCommand(IGenericRepository<SeatQEntities, ReadyMessage> rMsgRepository)
        {
            _rMsgRepository = rMsgRepository;
        }
        public void Handle(EditReadyMessageRequest request)
        {
            if (request.Model != null)
            {
                ReadyMessage rMsg = _rMsgRepository.FindBy(x => x.ReadyMessageId == request.Model.ReadyMessageId).FirstOrDefault();
                if (rMsg == null)
                {
                    rMsg = new ReadyMessage();
                    rMsg.RestaurantChainId = request.Model.RestaurantChainId;
                    rMsg.ReadyMessage1 = request.Model.ReadyMessage1;
                    rMsg.IsEnabled = request.Model.IsEnabled;
                    rMsg.IsDeleted = false;
                    rMsg.ModifiedDate = DateTime.UtcNow;
                    _rMsgRepository.Add(rMsg);
                    _rMsgRepository.Save();
                }
                else
                {
                    rMsg.ReadyMessage1 = request.Model.ReadyMessage1;
                    rMsg.IsEnabled = request.Model.IsEnabled;
                    rMsg.ModifiedDate = DateTime.UtcNow;
                    _rMsgRepository.Edit(rMsg);
                    _rMsgRepository.Save();
                }
            }
        }
    }
}
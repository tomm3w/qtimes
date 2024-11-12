using common.api.Commands;
using common.dal;
using SeatQ.core.dal.Models;
using System;
using System.Linq;

namespace SeatQ.core.api.Commands.EditVisitMessage
{
    public class EditVisitMessageCommand : ICommand<EditVisitMessageRequest>
    {
        private readonly IGenericRepository<SeatQEntities, VisitMessage> _vMsgRepository;
        public EditVisitMessageCommand(IGenericRepository<SeatQEntities, VisitMessage> vMsgRepository)
        {
            _vMsgRepository = vMsgRepository;
        }

        public void Handle(EditVisitMessageRequest request)
        {
            if (request.Model != null)
            {
                VisitMessage vMsg = _vMsgRepository.FindBy(x => x.VisitMessageId == request.Model.VisitMessageId).FirstOrDefault();
                if (vMsg == null)
                {
                    vMsg = new VisitMessage();
                    vMsg.RestaurantChainId = request.Model.RestaurantChainId;
                    vMsg.Visit = request.Model.Visit;
                    vMsg.VisitMessage1 = request.Model.VisitMessage1;
                    vMsg.IsEnabled = request.Model.IsEnabled;
                    vMsg.IsDeleted = false;
                    vMsg.ModifiedDate = DateTime.UtcNow;
                    _vMsgRepository.Add(vMsg);
                    _vMsgRepository.Save();
                }
                else
                {
                    vMsg.Visit = request.Model.Visit;
                    vMsg.VisitMessage1 = request.Model.VisitMessage1;
                    vMsg.IsEnabled = request.Model.IsEnabled;
                    vMsg.ModifiedDate = DateTime.UtcNow;
                    _vMsgRepository.Edit(vMsg);
                    _vMsgRepository.Save();
                }
            }
        }
    }
}
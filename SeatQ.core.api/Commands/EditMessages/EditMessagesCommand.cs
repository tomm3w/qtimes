using common.api.Commands;
using common.dal;
using SeatQ.core.dal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SeatQ.core.api.Commands.EditMessages
{
    public class EditMessagesCommand : ICommand<EditMessagesRequest>
    {
        private readonly IGenericRepository<SeatQEntities, ReadyMessage> _rMsgRepository;
        private readonly IGenericRepository<SeatQEntities, VisitMessage> _vMsgRepository;

        public EditMessagesCommand(IGenericRepository<SeatQEntities, ReadyMessage> rMsgRepository, IGenericRepository<SeatQEntities, VisitMessage> vMsgRepository)
        {
            _rMsgRepository = rMsgRepository;
            _vMsgRepository = vMsgRepository;
        }

        public void Handle(EditMessagesRequest request)
        {
            if (request.Model.ReadyMessage != null)
            {
                request.Model.ReadyMessage.ForEach(m => {
                    ReadyMessage rMsg = _rMsgRepository.FindBy(v => v.RestaurantChainId == request.Model.RestaurantChainId && v.ReadyMessageId == m.ReadyMessageId).FirstOrDefault();
                    if (rMsg == null)
                    {
                        rMsg = new ReadyMessage();
                        rMsg.RestaurantChainId = request.Model.RestaurantChainId;
                        rMsg.ReadyMessage1 = m.ReadyMessage1;
                        rMsg.IsEnabled = m.IsEnabled;
                        rMsg.IsDeleted = m.IsDeleted;
                        rMsg.ModifiedDate = DateTime.UtcNow;
                        _rMsgRepository.Add(rMsg);
                        _rMsgRepository.Save();
                    }
                    else
                    {
                        rMsg.ReadyMessage1 = m.ReadyMessage1;
                        rMsg.IsEnabled = m.IsEnabled;
                        rMsg.IsDeleted = m.IsDeleted;
                        rMsg.ModifiedDate = DateTime.UtcNow;
                        _rMsgRepository.Edit(rMsg);
                        _rMsgRepository.Save();
                    }
                });
            }


            if (request.Model.VisitMessage != null)
            {
                request.Model.VisitMessage.ForEach(m =>
                    {
                        VisitMessage vm = _vMsgRepository.FindBy(v => v.RestaurantChainId == request.Model.RestaurantChainId && v.VisitMessageId == m.VisitMessageId).FirstOrDefault();
                        if (vm == null)
                        {
                            vm = new VisitMessage();
                            vm.RestaurantChainId = request.Model.RestaurantChainId;
                            vm.Visit = m.Visit;
                            vm.VisitMessage1 = m.VisitMessage1;
                            vm.IsEnabled = m.IsEnabled;
                            vm.IsDeleted = m.IsDeleted;
                            vm.ModifiedDate = DateTime.UtcNow;
                            _vMsgRepository.Add(vm);
                            _vMsgRepository.Save();
                        }
                        else
                        {
                            vm.VisitMessage1 = m.VisitMessage1;
                            vm.Visit = m.Visit;
                            vm.IsEnabled = m.IsEnabled;
                            vm.IsDeleted = m.IsDeleted;
                            vm.ModifiedDate = DateTime.UtcNow;
                            _vMsgRepository.Edit(vm);
                            _vMsgRepository.Save();
                        }
                    }
                    );
            }

        }
    }
}
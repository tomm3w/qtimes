using common.api.Commands;
using common.dal;
using SeatQ.core.dal.Models;
using System;
using System.Linq;

namespace SeatQ.core.api.Commands.UpdateConfirmed
{
    public class UpdateConfirmedCommand : ICommand<UpdateConfirmedRequest>
    {
        private readonly IGenericRepository<SeatQEntities, WaitList> _waitListRepository;
        public UpdateConfirmedCommand(IGenericRepository<SeatQEntities, WaitList> waitListRepository)
        {
            _waitListRepository = waitListRepository;
        }
        public void Handle(UpdateConfirmedRequest request)
        {
            var wl = _waitListRepository.FindBy(x => x.WaitListId == request.Model.WaitListId).FirstOrDefault();
            if(wl!=null)
            {
                wl.MessageReply = 1; //1=confirmed
                wl.MessageReplyRecivedDatetime = DateTime.UtcNow;
                _waitListRepository.Edit(wl);
                _waitListRepository.Save();
            }
        }
    }
}
using common.api.Commands;
using common.dal;
using SeatQ.core.dal.Models;
using System;
using System.Linq;

namespace SeatQ.core.api.Commands.UpdateLeave
{
    public class UpdateLeaveCommand : ICommand<UpdateLeaveRequest>
    {
        private readonly IGenericRepository<SeatQEntities, WaitList> _waitListRepository;
        public UpdateLeaveCommand(IGenericRepository<SeatQEntities, WaitList> waitListRepository)
        {
            _waitListRepository = waitListRepository;
        }
        public void Handle(UpdateLeaveRequest request)
        {
            var wl = _waitListRepository.FindBy(x => x.WaitListId == request.Model.WaitListId).FirstOrDefault();
            if(wl!=null)
            {
                wl.MessageReply = 2; //2=leave/cancel
                wl.MessageReplyRecivedDatetime = DateTime.UtcNow;
                _waitListRepository.Edit(wl);
                _waitListRepository.Save();
            }
        }
    }
}
using common.api.Commands;
using common.dal;
using SeatQ.core.dal.Models;
using System;
using System.Linq;

namespace SeatQ.core.api.Commands.UpdateLeft
{
    public class UpdateLeftCommand : ICommand<UpdateLeftRequest>
    {
        private readonly IGenericRepository<SeatQEntities, WaitList> _waitListRepository;
        public UpdateLeftCommand(IGenericRepository<SeatQEntities, WaitList> waitListRepository)
        {
            _waitListRepository = waitListRepository;
        }
        public void Handle(UpdateLeftRequest request)
        {
            var wl = _waitListRepository.FindBy(x => x.WaitListId == request.Model.WaitListId).FirstOrDefault();
            if (wl != null)
            {
                wl.IsLeft = true;
                wl.LeftDateTime = DateTime.UtcNow;
                _waitListRepository.Edit(wl);
                _waitListRepository.Save();
            }
        }
    }
}
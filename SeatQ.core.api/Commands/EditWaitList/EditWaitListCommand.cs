using common.api.Commands;
using common.dal;
using SeatQ.core.dal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SeatQ.core.api.Commands.EditWaitList
{
    public class EditWaitListCommand : ICommand<EditWaitListRequest>
    {
        private readonly IGenericRepository<SeatQEntities, WaitList> _waitlistRepository;
        private readonly IGenericRepository<SeatQEntities, GuestInfo> _guestRepository;

        public EditWaitListCommand(IGenericRepository<SeatQEntities, WaitList> waitlistRepository, IGenericRepository<SeatQEntities, GuestInfo> guestRepository)
        {
            _waitlistRepository = waitlistRepository;
            _guestRepository = guestRepository;
        }

        public void Handle(EditWaitListRequest request)
        {
            var wl = _waitlistRepository.FindBy(x => x.WaitListId == request.Model.WaitListId).FirstOrDefault();

            if (wl != null)
            {
                wl.GroupSize = request.Model.GroupSize;
                wl.GuestTypeId = request.Model.GuestTypeId;
                wl.EstimatedDateTime = Convert.ToDateTime(request.Model.EstimatedDateTime).ToUniversalTime();
                wl.ActualDateTime = Convert.ToDateTime(request.Model.ActualDateTime).ToUniversalTime();
                wl.TableId = request.Model.TableId;
                wl.RoomNumber = request.Model.RoomNumber;
                wl.Comments = request.Model.Comments;
                wl.GuestName = request.Model.GuestName;
                _waitlistRepository.Edit(wl);
                _waitlistRepository.Save();

                var gi = _guestRepository.FindBy(x => x.GuestId == wl.GuestId).FirstOrDefault();
                if (gi != null)
                {
                    request.Model.MobileNumber = request.Model.MobileNumber.Trim().Replace(" ", "").Replace("(", "").Replace(")", "").Replace("+", "").Replace("-", "");
                    gi.GuestName = request.Model.GuestName;
                    gi.MobileNumber = request.Model.MobileNumber;
                    _guestRepository.Edit(gi);
                    _guestRepository.Save();
                }
            }
        }

    }
}
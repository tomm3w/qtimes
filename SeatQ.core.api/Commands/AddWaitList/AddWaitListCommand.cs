using common.api.Commands;
using common.dal;
using SeatQ.core.dal.Models;
using System;
using System.Linq;

namespace SeatQ.core.api.Commands.AddWaitList
{
    public class AddWaitListCommand : ICommand<AddWaitListRequest>
    {
        private readonly IGenericRepository<SeatQEntities, WaitList> _waitlistRepository;
        private readonly IGenericRepository<SeatQEntities, GuestInfo> _guestRepository;

        public AddWaitListCommand(IGenericRepository<SeatQEntities, WaitList> waitlistRepository, IGenericRepository<SeatQEntities, GuestInfo> guestRepository)
        {
            _waitlistRepository = waitlistRepository;
            _guestRepository = guestRepository;
        }

        public void Handle(AddWaitListRequest request)
        {
            request.Model.MobileNumber = request.Model.MobileNumber.Trim().Replace(" ","").Replace("(", "").Replace(")", "").Replace("+", "").Replace("-","");
            var guest = _guestRepository.FindBy(x => x.MobileNumber == request.Model.MobileNumber && x.RestaurantChainId == request.Model.RestaurantChainId).FirstOrDefault();
            if (guest == null)
            {
                guest = new GuestInfo();
                guest.GuestName = request.Model.GuestName;
                guest.MobileNumber = request.Model.MobileNumber;
                guest.NoOfReturn = 0;
                guest.RestaurantChainId = request.Model.RestaurantChainId;
                _guestRepository.Add(guest);
                _guestRepository.Save();
            }

            var wlist = new WaitList()
            {
                RestaurantChainId = request.Model.RestaurantChainId,
                GuestId = guest.GuestId,
                GroupSize = request.Model.GroupSize,
                GuestTypeId = request.Model.GuestTypeId,
                EnteredBy = request.UserId,
                EnteredDateTime = DateTime.UtcNow,
                EstimatedDateTime = Convert.ToDateTime(request.Model.EstimatedDateTime).ToUniversalTime(),
                ActualDateTime = Convert.ToDateTime(request.Model.ActualDateTime).ToUniversalTime(),
                TableId = request.Model.TableId,
                RoomNumber = request.Model.RoomNumber,
                IsMessageSent = false,
                IsSeated = false,
                IsLeft = false,
                DeviceId = request.Model.DeviceId,
                Comments = request.Model.Comments,
                Visit = guest.NoOfReturn,
                GuestName = request.Model.GuestName
            };
            _waitlistRepository.Add(wlist);
            _waitlistRepository.Save();
        }
    }
}
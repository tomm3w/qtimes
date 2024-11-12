using common.api.Commands;
using common.dal;
using SeatQ.core.dal.Models;
using System;
using System.Linq;

namespace SeatQ.core.api.Commands.UpdateSeated
{
    public class UpdateSeatedCommand : ICommand<UpdateSeatedRequest>
    {
        private readonly IGenericRepository<SeatQEntities, WaitList> _waitListRepository;
        private readonly IGenericRepository<SeatQEntities, GuestInfo> _guestInfoRepository;
        private readonly IGenericRepository<SeatQEntities, VisitMessage> _visitMessage;
        private readonly IGenericRepository<SeatQEntities, VisitMessageSent> _visitMessageSent;
        public UpdateSeatedCommand(IGenericRepository<SeatQEntities, WaitList> waitListRepository, IGenericRepository<SeatQEntities, GuestInfo> guestInfoRepository, IGenericRepository<SeatQEntities, VisitMessage> visitMessage, IGenericRepository<SeatQEntities, VisitMessageSent> visitMessageSent)
        {
            _waitListRepository = waitListRepository;
            _guestInfoRepository = guestInfoRepository;
            _visitMessage = visitMessage;
            _visitMessageSent = visitMessageSent;
        }
        public void Handle(UpdateSeatedRequest request)
        {
            var wl = _waitListRepository.FindBy(x => x.WaitListId == request.Model.WaitListId).FirstOrDefault();
            if (wl != null && wl.IsSeated != true)
            {
                var gi = _guestInfoRepository.FindBy(g => g.GuestId == wl.GuestId).FirstOrDefault();

                wl.IsSeated = true;
                wl.TableId = request.Model.TableId;
                wl.SeatedDateTime = DateTime.UtcNow;
                if (gi != null)
                    wl.Visit = gi.NoOfReturn + 1;
                _waitListRepository.Edit(wl);
                _waitListRepository.Save();

                if (gi != null)
                {
                    gi.NoOfReturn = gi.NoOfReturn + 1;
                    _guestInfoRepository.Edit(gi);
                    _guestInfoRepository.Save();

                    //Set message for visit
                    //1. Check if message is defined for no. of visit
                    //2. If yes, insert into visitmessagesent table for further processing
                    //if (IsVisitMessageExists((int)wl.RestaurantChainId, (int)gi.NoOfReturn))
                    //{
                    //    VisitMessageSent v = context.VisitMessageSents.Create();
                    //    v.GuestId = wl.GuestId;
                    //    v.RestaurantChainId = wl.RestaurantChainId;
                    //    v.Visit = gi.NoOfReturn;
                    //    context.VisitMessageSents.Add(v);
                    //    context.SaveChanges();
                    //}

                    //var vi = _visitMessage.FindBy(m => m.RestaurantChainId == wl.RestaurantChainId && m.Visit == gi.NoOfReturn && m.IsDeleted != true && m.IsEnabled == true).FirstOrDefault();
                    //if (vi == null)
                    //{
                    //    vi = _visitMessage.FindBy(m => m.RestaurantChainId == wl.RestaurantChainId && m.Visit == 1 && m.IsDeleted != true && m.IsEnabled == true).FirstOrDefault();
                    //}

                    //if (vi != null)
                    //{
                    //    var vms = new VisitMessageSent()
                    //    {
                    //        GuestId = gi.GuestId,
                    //        RestaurantChainId = wl.RestaurantChainId,
                    //        Visit = gi.NoOfReturn,
                    //        MessageText = vi.VisitMessage1
                    //    };
                    //    _visitMessageSent.Add(vms);
                    //    _visitMessageSent.Save();
                    //}

                }
            }
        }
    }
}
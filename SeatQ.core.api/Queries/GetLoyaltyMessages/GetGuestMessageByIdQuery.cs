using common.api.Queries;
using common.dal;
using SeatQ.core.dal.Models;
using System.Linq;

namespace SeatQ.core.api.Queries.GetLoyaltyMessages
{
    public class GetLoyaltyMessageByIdQuery : IQuery<GetLoyaltyMessageByIdResponse, GetLoyaltyMessageByIdRequest>
    {
        private readonly IGenericRepository<SeatQEntities, LoyaltyMessage> _repository;
        public GetLoyaltyMessageByIdQuery(IGenericRepository<SeatQEntities, LoyaltyMessage> repository)
        {
            _repository = repository;
        }
        public GetLoyaltyMessageByIdResponse Handle(GetLoyaltyMessageByIdRequest request)
        {
            var wl = _repository.FindBy(x => x.GuestId == request.GuestId).ToList();
            if (wl != null)
            {
                foreach (LoyaltyMessage g in wl)
                {
                    if(g.IsMessageRead != true)
                    {
                        g.IsMessageRead = true;
                        _repository.Edit(g);
                        _repository.Save();
                    }
                }

                return new GetLoyaltyMessageByIdResponse(wl);
            }
            return null;
        }
    }
}
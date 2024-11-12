using common.api.Queries;
using common.dal;
using SeatQ.core.api.Queries.GetGuestMessages;
using SeatQ.core.dal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SeatQ.core.api.Queries.GetGuestMessages
{
    public class GetGuestMessageByIdQuery : IQuery<GetGuestMessageByIdResponse, GetGuestMessageByIdRequest>
    {
        private readonly IGenericRepository<SeatQEntities, GuestMessage> _repository;
        public GetGuestMessageByIdQuery(IGenericRepository<SeatQEntities, GuestMessage> repository)
        {
            _repository = repository;
        }
        public GetGuestMessageByIdResponse Handle(GetGuestMessageByIdRequest request)
        {
            var wl = _repository.FindBy(x => x.WaitListId == request.WaitListId).ToList();
            if (wl != null)
            {
                foreach (GuestMessage g in wl)
                {
                    if(g.IsMessageRead != true)
                    {
                        g.IsMessageRead = true;
                        _repository.Edit(g);
                        _repository.Save();
                    }
                }

                return new GetGuestMessageByIdResponse(wl);
            }
            return null;
        }
    }
}
using common.api.Queries;
using common.dal;
using SeatQ.core.api.Queries.GetWaitList;
using SeatQ.core.dal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SeatQ.core.api.Queries.GetGuestById
{
    public class GetGuestByIdQuery : IQuery<GetGuestByIdResponse, GetGuestByIdRequest>
	{
		private readonly IGenericRepository<SeatQEntities, GuestInfo> _repository;

        public GetGuestByIdQuery(IGenericRepository<SeatQEntities, GuestInfo> repository)
        {
            _repository = repository;
        }
        public GetGuestByIdResponse Handle(GetGuestByIdRequest request)
        {
            var wl = _repository.FindBy(x => x.GuestId == request.GuestId).FirstOrDefault();
            if (wl != null)
            {
                return new GetGuestByIdResponse(wl);
            }
            return null;
        }
    }
}
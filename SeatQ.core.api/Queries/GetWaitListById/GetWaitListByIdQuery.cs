using common.api.Queries;
using common.dal;
using SeatQ.core.api.Queries.GetWaitList;
using SeatQ.core.dal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SeatQ.core.api.Queries.GetWaitListById
{
    public class GetWaitListByIdQuery : IQuery<GetWaitListByIdResponse, GetWaitListByIdRequest>
	{
		private readonly IGenericRepository<SeatQEntities, WaitList> _repository;

        public GetWaitListByIdQuery(IGenericRepository<SeatQEntities, WaitList> repository)
        {
            _repository = repository;
        }
        public GetWaitListByIdResponse Handle(GetWaitListByIdRequest request)
        {
            var wl = _repository.FindBy(x => x.WaitListId == request.WaitListId).FirstOrDefault();
            if (wl != null)
            {
                return new GetWaitListByIdResponse(wl);
            }
            return null;
        }
    }
}
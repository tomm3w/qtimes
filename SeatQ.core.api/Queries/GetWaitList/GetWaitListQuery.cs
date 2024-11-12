using common.api.Queries;
using common.dal;
using SeatQ.core.dal.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Web;

namespace SeatQ.core.api.Queries.GetWaitList
{
    public class GetWaitListQuery : IQuery<GetWaitListResponse, GetWaitListRequest>
    {
        private readonly IGenericRepository<SeatQEntities, WaitList> _waitListRepository;

        public GetWaitListQuery(IGenericRepository<SeatQEntities, WaitList> waitListRepository)
        {
            waitListRepository = _waitListRepository;
        }

        public GetWaitListResponse Handle(GetWaitListRequest request)
        {
            return null;
        }
    }
}
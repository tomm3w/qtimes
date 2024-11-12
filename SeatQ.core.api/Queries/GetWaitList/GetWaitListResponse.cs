using common.api.Queries;
using SeatQ.core.dal.Models;
using System.Collections.Generic;

namespace SeatQ.core.api.Queries.GetWaitList
{
    public class GetWaitListResponse : IQueryResponse
    {
        public List<WaitList> Model { get; private set; }
        public PagingModel PagingModel { get; set; }
        public GetWaitListResponse(List<WaitList> model, PagingModel pagingModel)
        {
            Model = model;
            PagingModel = pagingModel;
        }
    }
}
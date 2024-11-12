using common.api.Queries;

namespace SeatQ.core.api.Queries.GetWaitList
{
    public class GetWaitListRequest : IQueryRequest
    {
        public int? Page { get; set; }
        public int? PageSize { get; set; }
        public int RestaurantChainId { get; set; }
        public GetWaitListRequest()
        { }
        public GetWaitListRequest(int? page, int? pageSize, int restaurantChainId)
        {
            Page = page;
            PageSize = pageSize;
            RestaurantChainId = restaurantChainId;
        }
    }
}
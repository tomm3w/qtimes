using common.api.Queries;

namespace SeatQ.core.api.Queries.GetHostess
{
    public class GetHostessRequest : IQueryRequest
    {
        public GetHostessRequest(int restaurantChainId)
        {
            RestaurantChainId = restaurantChainId;
        }

        public int RestaurantChainId { get; private set; }
    }

}
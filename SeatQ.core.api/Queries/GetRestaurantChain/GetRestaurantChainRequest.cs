using common.api.Queries;

namespace SeatQ.core.api.Queries.GetRestaurant
{
    public class GetRestaurantChainRequest : IQueryRequest
    {
        public GetRestaurantChainRequest(int restaurantChainId)
		{
            RestaurantChainId = restaurantChainId;
		}

        public int RestaurantChainId { get; private set; }
    }
}
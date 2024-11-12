using common.api.Queries;

namespace SeatQ.core.api.Queries.GetRestaurant
{
    public class GetRestaurantRequest : IQueryRequest
    {
        public GetRestaurantRequest(int restaurantId)
		{
            RestaurantId = restaurantId;
		}

        public int RestaurantId { get; private set; }
    }
}
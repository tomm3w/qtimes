using common.api.Queries;

namespace SeatQ.core.api.Queries.GetRestaurantChains
{
    public class GetRestaurantChainsRequest : IQueryRequest
    {
        public GetRestaurantChainsRequest(int restaurantId)
        {
            RestaurantId = restaurantId;
        }
        public int RestaurantId { get; private set; }
    }
}
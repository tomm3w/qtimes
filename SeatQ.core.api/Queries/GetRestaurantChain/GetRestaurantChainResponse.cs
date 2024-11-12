using common.api.Queries;
using SeatQ.core.common.Dto;
using SeatQ.core.dal.Models;

namespace SeatQ.core.api.Queries.GetRestaurantChain
{
    public class GetRestaurantChainResponse : IQueryResponse
    {
        public RestaurantChain RestaurantChain { get; private set; }

        public GetRestaurantChainResponse(RestaurantChain restaurantChain)
        {
            RestaurantChain = restaurantChain;
        }

    }
}
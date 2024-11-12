using common.api.Queries;
using SeatQ.core.common.Dto;
using SeatQ.core.dal.Models;
using System.Collections.Generic;

namespace SeatQ.core.api.Queries.GetRestaurantChains
{
    public class GetRestaurantChainsResponse : IQueryResponse
    {
        public List<RestaurantChain> RestaurantChain { get; private set; }

        public GetRestaurantChainsResponse(List<RestaurantChain> restaurantChain)
        {
            RestaurantChain = restaurantChain;
        }

    }
}
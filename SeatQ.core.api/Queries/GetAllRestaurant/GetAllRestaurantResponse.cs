using common.api.Queries;
using SeatQ.core.dal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SeatQ.core.api.Queries.GetAllRestaurant
{
    public class GetAllRestaurantResponse : IQueryResponse
    {
        public List<RestaurantChain> RestaurantChains { get; private set; }
        public GetAllRestaurantResponse(List<RestaurantChain> restaurantChains)
        {
            RestaurantChains = restaurantChains;
        }
    }
}
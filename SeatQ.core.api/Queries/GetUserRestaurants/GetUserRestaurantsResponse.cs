using common.api.Queries;
using SeatQ.core.dal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SeatQ.core.api.Queries.GetUserRestaurants
{
    public class GetUserRestaurantsResponse : IQueryResponse
    {
        public List<UsersInRestaurant> UsersInRestaurant { get; private set; }
        public GetUserRestaurantsResponse(List<UsersInRestaurant> usersInRestaurant)
        {
            UsersInRestaurant = usersInRestaurant;
        }
    }
}
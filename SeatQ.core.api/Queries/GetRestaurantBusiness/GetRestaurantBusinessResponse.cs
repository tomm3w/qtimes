using common.api.Queries;
using SeatQ.core.dal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SeatQ.core.api.Queries.GetRestaurantBusiness
{
    public class GetRestaurantBusinessResponse : IQueryResponse
    {
        public UsersInRestaurant UsersInRestaurant { get; private set; }

        public GetRestaurantBusinessResponse(UsersInRestaurant usersInRestaurant)
        {
            UsersInRestaurant = usersInRestaurant;
        }
    }
}
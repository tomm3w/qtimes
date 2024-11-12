using common.api.Queries;
using common.dal;
using SeatQ.core.dal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SeatQ.core.api.Queries.GetUserRestaurants
{
    public class GetUserRestaurantsQuery : IQuery<GetUserRestaurantsResponse, GetUserRestaurantsRequest>
    {
        private readonly IGenericRepository<SeatQEntities, UsersInRestaurant> _usersInRestaurantRepository;
        public GetUserRestaurantsQuery(IGenericRepository<SeatQEntities, UsersInRestaurant> usersInRestaurantRepository)
        {
            _usersInRestaurantRepository = usersInRestaurantRepository;
        }
        public GetUserRestaurantsResponse Handle(GetUserRestaurantsRequest request)
        {
            var restaurantChain = _usersInRestaurantRepository.FindBy(x => x.UserId == request.UserId).ToList();
            if (restaurantChain != null)
            {
                return new GetUserRestaurantsResponse(restaurantChain.ToList());
            }

            return null;
        }
    }
}
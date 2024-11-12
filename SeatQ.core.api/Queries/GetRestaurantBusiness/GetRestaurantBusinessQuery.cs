using common.api.Queries;
using common.dal;
using SeatQ.core.dal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SeatQ.core.api.Queries.GetRestaurantBusiness
{
    public class GetRestaurantBusinessQuery : IQuery<GetRestaurantBusinessResponse, GetRestaurantBusinessRequest>
    {
        private readonly IGenericRepository<SeatQEntities, UsersInRestaurant> _usersInRestaurant;

        public GetRestaurantBusinessQuery(IGenericRepository<SeatQEntities, UsersInRestaurant> usersInRestaurant)
        {
            _usersInRestaurant = usersInRestaurant;
        }

        public GetRestaurantBusinessResponse Handle(GetRestaurantBusinessRequest request)
        {
            var restaurantChain = _usersInRestaurant.FindBy(x => x.BusinessId == request.BusinessId && x.UserId == request.UserId).FirstOrDefault();
            if (restaurantChain != null)
            {
                return new GetRestaurantBusinessResponse(restaurantChain);
            }

            return null;
        }
    }
}
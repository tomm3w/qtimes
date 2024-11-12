using common.api.Queries;
using SeatQ.core.common.Dto;
using SeatQ.core.dal.Models;

namespace SeatQ.core.api.Queries.GetRestaurantByBusinessId
{
    public class GetRestaurantByBusinessIdResponse : IQueryResponse
    {
        public UsersInRestaurant Restaurant { get; private set; }

        public GetRestaurantByBusinessIdResponse(UsersInRestaurant restaurant)
        {
            Restaurant = restaurant;
        }

    }
}
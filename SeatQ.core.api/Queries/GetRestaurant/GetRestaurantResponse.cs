using common.api.Queries;
using SeatQ.core.common.Dto;

namespace SeatQ.core.api.Queries.GetRestaurant
{
    public class GetRestaurantResponse : IQueryResponse
    {
        public RestaurantDto Restaurant { get; private set; }

        public GetRestaurantResponse(RestaurantDto restaurant)
        {
            Restaurant = restaurant;
        }

    }
}
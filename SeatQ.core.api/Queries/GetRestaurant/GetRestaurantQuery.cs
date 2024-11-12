using System.Linq;
using AutoMapper;
using common.api.Queries;
using common.dal;
using SeatQ.core.dal.Models;
using SeatQ.core.common.Dto;

namespace SeatQ.core.api.Queries.GetRestaurant
{
    public class GetRestaurantQuery : IQuery<GetRestaurantResponse, GetRestaurantRequest>
    {
        private readonly IGenericRepository<SeatQEntities, Restaurant> _restaurantRepository;

        public GetRestaurantQuery(IGenericRepository<SeatQEntities, Restaurant> restaurantRepository)
        {
            _restaurantRepository = restaurantRepository;
        }

        public GetRestaurantResponse Handle(GetRestaurantRequest request)
        {
            var restaurant = _restaurantRepository.FindBy(x => x.RestaurantId == request.RestaurantId).FirstOrDefault();
            if (restaurant != null)
            {
                var dto = Mapper.Map<Restaurant, RestaurantDto>(restaurant);
                return new GetRestaurantResponse(dto);
            }

            return null;
        }
    }
}
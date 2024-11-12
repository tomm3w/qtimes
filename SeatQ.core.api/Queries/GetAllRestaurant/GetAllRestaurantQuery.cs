using common.api.Queries;
using common.dal;
using SeatQ.core.dal.Models;
using System.Linq;

namespace SeatQ.core.api.Queries.GetAllRestaurant
{
    public class GetAllRestaurantQuery : IQuery<GetAllRestaurantResponse, GetAllRestaurantRequest>
    {
        private readonly IGenericRepository<SeatQEntities, RestaurantChain> _restaurantChainRepository;
        public GetAllRestaurantQuery(IGenericRepository<SeatQEntities, RestaurantChain> restaurantChainRepository)
        {
            _restaurantChainRepository = restaurantChainRepository;
        }
        public GetAllRestaurantResponse Handle(GetAllRestaurantRequest request)
        {
            var restaurantChain = _restaurantChainRepository.GetAll();
            return new GetAllRestaurantResponse(restaurantChain.ToList());
            
        }
    }
}
using System.Linq;
using AutoMapper;
using common.api.Queries;
using common.dal;
using SeatQ.core.dal.Models;
using SeatQ.core.common.Dto;
using SeatQ.core.api.Queries.GetRestaurantChain;

namespace SeatQ.core.api.Queries.GetRestaurant
{
    public class GetRestaurantChainQuery : IQuery<GetRestaurantChainResponse, GetRestaurantChainRequest>
    {
        private readonly IGenericRepository<SeatQEntities, RestaurantChain> _restaurantChainRepository;

        public GetRestaurantChainQuery(IGenericRepository<SeatQEntities, RestaurantChain> restaurantChainRepository)
        {
            _restaurantChainRepository = restaurantChainRepository;
        }

        public GetRestaurantChainResponse Handle(GetRestaurantChainRequest request)
        {
            var restaurantChain = _restaurantChainRepository.FindBy(x => x.RestaurantChainId == request.RestaurantChainId).FirstOrDefault();
            if (restaurantChain != null)
            {
                return new GetRestaurantChainResponse(restaurantChain);
            }

            return null;
        }
    }
}
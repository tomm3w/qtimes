using System.Linq;
using AutoMapper;
using common.api.Queries;
using common.dal;
using SeatQ.core.dal.Models;
using SeatQ.core.common.Dto;
using SeatQ.core.api.Queries.GetRestaurantChain;

namespace SeatQ.core.api.Queries.GetRestaurantChains
{
    public class GetRestaurantChainsQuery : IQuery<GetRestaurantChainsResponse, GetRestaurantChainsRequest>
    {
        private readonly IGenericRepository<SeatQEntities, RestaurantChain> _restaurantChainsRepository;

        public GetRestaurantChainsQuery(IGenericRepository<SeatQEntities, RestaurantChain> restaurantChainsRepository)
        {
            _restaurantChainsRepository = restaurantChainsRepository;
        }

        public GetRestaurantChainsResponse Handle(GetRestaurantChainsRequest request)
        {
            var restaurantChain = _restaurantChainsRepository.FindBy(x => x.RestaurantId == request.RestaurantId).ToList();
            if (restaurantChain != null)
            {
                return new GetRestaurantChainsResponse(restaurantChain.ToList());
            }

            return null;
        }
    }
}
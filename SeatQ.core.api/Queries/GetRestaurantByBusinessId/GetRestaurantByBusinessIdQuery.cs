using System.Linq;
using AutoMapper;
using common.api.Queries;
using common.dal;
using SeatQ.core.dal.Models;
using SeatQ.core.common.Dto;
using SeatQ.core.api.Queries.GetRestaurantChain;

namespace SeatQ.core.api.Queries.GetRestaurantByBusinessId
{
    public class GetRestaurantByBusinessIdQuery : IQuery<GetRestaurantByBusinessIdResponse, GetRestaurantByBusinessIdRequest>
    {
        private readonly IGenericRepository<SeatQEntities, UsersInRestaurant> _restaurantRepository;

        public GetRestaurantByBusinessIdQuery(IGenericRepository<SeatQEntities, UsersInRestaurant> restaurantRepository)
        {
            _restaurantRepository = restaurantRepository;
        }

        public GetRestaurantByBusinessIdResponse Handle(GetRestaurantByBusinessIdRequest request)
        {
            var restaurantChain = _restaurantRepository.FindBy(x => x.BusinessId == request.BusinessId).FirstOrDefault();
            if (restaurantChain != null)
            {
                return new GetRestaurantByBusinessIdResponse(restaurantChain);
            }

            return null;
        }
    }
}
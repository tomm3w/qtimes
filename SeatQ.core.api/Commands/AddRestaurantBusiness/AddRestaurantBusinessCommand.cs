using common.api.Commands;
using common.dal;
using SeatQ.core.dal.Models;
using System;
using System.Linq;

namespace SeatQ.core.api.Commands.AddRestaurantBusiness
{
    public class AddRestaurantBusinessCommand : ICommand<AddRestaurantBusinessRequest>
    {
        private readonly IGenericRepository<SeatQEntities, Restaurant> _restaurantRepository;
        private readonly IGenericRepository<SeatQEntities, RestaurantChain> _restaurantChainRepository;
        private readonly IGenericRepository<SeatQEntities, UserInfo> _userInfoRepository;
        private readonly IGenericRepository<SeatQEntities, UsersInRestaurant> _userInRestaurantRepository;

        public AddRestaurantBusinessCommand(IGenericRepository<SeatQEntities, Restaurant> restaurantRepository, IGenericRepository<SeatQEntities, RestaurantChain> restaurantChainRepository, IGenericRepository<SeatQEntities, UserInfo> userInfoRepository, IGenericRepository<SeatQEntities, UsersInRestaurant> userInRestaurantRepository)
        {
            _restaurantRepository = restaurantRepository;
            _restaurantChainRepository = restaurantChainRepository;
            _userInfoRepository = userInfoRepository;
            _userInRestaurantRepository = userInRestaurantRepository;
        }

        public void Handle(AddRestaurantBusinessRequest request)
        {
            var usrRestaurant = _userInRestaurantRepository.FindBy(x => x.UserId == request.Model.UserId && x.BusinessId == request.Model.BusinessId).FirstOrDefault();
            if(usrRestaurant == null)
            {
                //Insert in Restaurant
                var restaurant = new Restaurant
                {
                    BusinessName = request.Model.BusinessName,
                    SignUpDate = DateTime.UtcNow,
                    ConfirmationToken = Guid.NewGuid(),
                    isConfirmed = true,
                    ConfirmedDate = DateTime.UtcNow
                };

                _restaurantRepository.Add(restaurant);
                _restaurantRepository.Save();

                //Insert in RestaurantChain
                var chain = new RestaurantChain
                {
                    RestaurantId = restaurant.RestaurantId,
                    IsActive = true,
                    ModifiedDate = DateTime.UtcNow
                };

                _restaurantChainRepository.Add(chain);
                _restaurantChainRepository.Save();

                var business = new UsersInRestaurant
                {
                    UserId = request.Model.UserId,
                    BusinessId = request.Model.BusinessId,
                    RestaurantChainId =chain.RestaurantChainId
                };
                _userInRestaurantRepository.Add(business);
                _userInRestaurantRepository.Save();
            }
        }

    }
}
using common.api.Commands;
using common.dal;
using SeatQ.core.dal.Models;
using System;

namespace SeatQ.core.api.Commands.AddAccount
{
    public class AddAccountCommand : ICommand<AddAccountRequest>
    {
        private readonly IGenericRepository<SeatQEntities, Restaurant> _restaurantRepository;
        private readonly IGenericRepository<SeatQEntities, RestaurantChain> _restaurantChainRepository;
        private readonly IGenericRepository<SeatQEntities, UserInfo> _userInfoRepository;
        public AddAccountCommand(IGenericRepository<SeatQEntities, Restaurant> restaurantRepository, IGenericRepository<SeatQEntities, RestaurantChain> restaurantChainRepository, IGenericRepository<SeatQEntities, UserInfo> userInfoRepository)
        {
            _restaurantRepository = restaurantRepository;
            _restaurantChainRepository = restaurantChainRepository;
            _userInfoRepository = userInfoRepository;
        }
        public void Handle(AddAccountRequest request)
        {
            var restaurant = new Restaurant
            {
                BusinessName = request.Model.BusinessName,
                SignUpDate = DateTime.UtcNow,
                PlanId = 1,//TODO:add user selected plan type
                ConfirmationToken = Guid.NewGuid(),
                isConfirmed = true
            };
            _restaurantRepository.Add(restaurant);
            _restaurantRepository.Save();

            var restaurantChain = new RestaurantChain
            {
                RestaurantId = restaurant.RestaurantId,
                FullName = request.Model.FullName,
                Email = request.Model.Email,
                Address1 = request.Model.Address1,
                Address2 = request.Model.Address2,
                City = request.Model.CityTown,
                State = request.Model.State,
                Zip = request.Model.Zip,
                Phone = request.Model.Phone,
                IsActive = true
            };
            _restaurantChainRepository.Add(restaurantChain);
            _restaurantChainRepository.Save();

            var userInfo = new UserInfo
            {
                UserId = (Guid)request.Model.UserId,
                RestaurantChainId = restaurantChain.RestaurantChainId,
                isActive = true,
                isDeleted = false
            };
            _userInfoRepository.Add(userInfo);
            _userInfoRepository.Save();
        }
    }
}
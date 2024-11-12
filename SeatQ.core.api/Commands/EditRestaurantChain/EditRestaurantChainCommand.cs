using common.api.Commands;
using common.dal;
using SeatQ.core.dal.Models;
using System;
using System.Linq;

namespace SeatQ.core.api.Commands.EditRestaurantChain
{
    public class EditRestaurantChainCommand : ICommand<EditRestaurantChainRequest>
    {
        private readonly IGenericRepository<SeatQEntities, Restaurant> _restaurantRepository;
        private readonly IGenericRepository<SeatQEntities, RestaurantChain> _restaurantChainRepository;

        public EditRestaurantChainCommand(IGenericRepository<SeatQEntities, Restaurant> restaurantRepository, IGenericRepository<SeatQEntities, RestaurantChain> restaurantChainRepository)
        {
            _restaurantRepository = restaurantRepository;
            _restaurantChainRepository = restaurantChainRepository;
        }
        public void Handle(EditRestaurantChainRequest request)
        {
            var restaurant = _restaurantRepository.FindBy(x => x.RestaurantId == request.Model.RestaurantId).FirstOrDefault();
            if(restaurant!=null)
            {
                restaurant.BusinessName = request.Model.BusinessName;
                _restaurantRepository.Edit(restaurant);
                _restaurantRepository.Save();
            }

            var restaurantChain = _restaurantChainRepository.FindBy(x => x.RestaurantChainId == request.Model.RestaurantChainId).FirstOrDefault();
            if(restaurantChain!=null)
            {
                restaurantChain.FullName = request.Model.FullName;
                restaurantChain.Email = request.Model.Email;
                restaurantChain.Address1 = request.Model.Address1;
                restaurantChain.Address2 = request.Model.Address2;
                restaurantChain.City = request.Model.CityTown;
                restaurantChain.State = request.Model.State;
                restaurantChain.Zip = request.Model.Zip;
                restaurantChain.Phone = request.Model.Phone;
                restaurantChain.RestaurantNumber = request.Model.RestaurantNumber;
                restaurantChain.ModifiedDate = DateTime.UtcNow;
                _restaurantChainRepository.Edit(restaurantChain);
                _restaurantChainRepository.Save();
            }
        }

    }
}
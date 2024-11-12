using common.api.Commands;
using common.dal;
using SeatQ.core.dal.Models;
using System.Linq;

namespace SeatQ.core.api.Commands.AssignStaff
{
    public class AssignStaffCommand : ICommand<AssignStaffRequest>
    {
        private readonly IGenericRepository<SeatQEntities, RestaurantTable> _restaurantRepository;
        public AssignStaffCommand(IGenericRepository<SeatQEntities, RestaurantTable> restaurantRepository)
        {
            _restaurantRepository = restaurantRepository;
        }
        public void Handle(AssignStaffRequest request)
        {
            var restaurant = _restaurantRepository.FindBy(x => x.RestaurantChainId == request.Model.RestaurantChainId && x.TableId == request.Model.TableId).FirstOrDefault();
            if (restaurant != null)
            {
                restaurant.AssignedTo = request.Model.UserId;
                _restaurantRepository.Edit(restaurant);
                _restaurantRepository.Save();
            }
        }

    }

}
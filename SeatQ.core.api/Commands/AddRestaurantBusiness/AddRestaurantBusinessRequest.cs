using common.api.Commands;
using SeatQ.core.dal.Models;

namespace SeatQ.core.api.Commands.AddRestaurantBusiness
{
    public class AddRestaurantBusinessRequest : ICommandRequest
    {
        public RestaurantBusinessModel Model { get; private set; }
        public AddRestaurantBusinessRequest(RestaurantBusinessModel model)
        {
            Model = model;
        }
    }
}
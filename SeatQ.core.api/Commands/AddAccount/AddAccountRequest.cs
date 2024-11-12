using common.api.Commands;
using SeatQ.core.dal.Models;

namespace SeatQ.core.api.Commands.AddAccount
{
    public class AddAccountRequest : ICommandRequest
    {
        public RestaurantInfoModel Model { get; private set; }
        public AddAccountRequest(RestaurantInfoModel model)
		{
            Model = model;
		}
    }
}
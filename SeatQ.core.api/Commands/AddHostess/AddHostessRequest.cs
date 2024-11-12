using common.api.Commands;
using SeatQ.core.dal.Models;

namespace SeatQ.core.api.Commands.AddHostess
{
    public class AddHostessRequest : ICommandRequest
    {
        public HostessModel Model { get; private set; }
        public AddHostessRequest(HostessModel model)
		{
            Model = model;
		}
    }
}
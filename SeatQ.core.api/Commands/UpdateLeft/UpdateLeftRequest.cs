using common.api.Commands;
using SeatQ.core.dal.Models;

namespace SeatQ.core.api.Commands.UpdateLeft
{
    public class UpdateLeftRequest : ICommandRequest
    {
        public WaitListIdModel Model { get; set; }

        public UpdateLeftRequest(WaitListIdModel model)
        {
            Model = model;
        }
    }
}
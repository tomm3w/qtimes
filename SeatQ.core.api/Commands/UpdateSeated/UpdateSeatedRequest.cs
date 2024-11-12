using common.api.Commands;
using SeatQ.core.dal.Models;

namespace SeatQ.core.api.Commands.UpdateSeated
{
    public class UpdateSeatedRequest : ICommandRequest
    {
        public WaitListSeatedModel Model { get; set; }

        public UpdateSeatedRequest(WaitListSeatedModel model)
        {
            Model = model;
        }
    }
}
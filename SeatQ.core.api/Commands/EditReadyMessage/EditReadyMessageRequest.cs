using common.api.Commands;
using SeatQ.core.dal.Models;

namespace SeatQ.core.api.Commands.EditReadyMessage
{
    public class EditReadyMessageRequest: ICommandRequest
    {
        public ReadyMessage Model { get; private set; }
        public EditReadyMessageRequest(ReadyMessage model)
        {
            Model = model;
        }
    }
}
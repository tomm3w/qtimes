using common.api.Commands;
using SeatQ.core.dal.Models;

namespace SeatQ.core.api.Commands.EditVisitMessage
{
    public class EditVisitMessageRequest : ICommandRequest
    {
        public VisitMessage Model { get; private set; }
        public EditVisitMessageRequest(VisitMessage model)
        {
            Model = model;
        }
    }
}
using common.api.Commands;
using System;

namespace SeatQ.core.api.Commands.DeleteVisitMessage
{
    public class DeleteVisitMessageRequest : ICommandRequest
    {
        public int VisitMessageId { get; private set; }
        public DeleteVisitMessageRequest(int id)
        {
            VisitMessageId = id;
        }
    }
}
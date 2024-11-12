using common.api.Commands;
using System;

namespace SeatQ.core.api.Commands.DeleteUser
{
    public class DeleteUserRequest : ICommandRequest
    {
        public Guid Id { get; private set; }
        public DeleteUserRequest(Guid id)
        {
            Id = id;
        }
    }
}
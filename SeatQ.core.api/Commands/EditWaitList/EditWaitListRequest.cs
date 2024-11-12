using common.api.Commands;
using SeatQ.core.dal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SeatQ.core.api.Commands.EditWaitList
{
    public class EditWaitListRequest : ICommandRequest
    {
        public InsertWaitList Model { get; private set; }
        public Guid UserId { get; private set; }
        public EditWaitListRequest(InsertWaitList model, Guid userId)
        {
            Model = model;
            UserId = userId;
        }
    }
}
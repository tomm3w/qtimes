using common.api.Commands;
using SeatQ.core.dal.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SeatQ.core.api.Commands.AddWaitList
{
    public class AddWaitListRequest : ICommandRequest
    {
        public InsertWaitList Model { get; private set; }
        public Guid UserId { get; private set; }

        public AddWaitListRequest(InsertWaitList model, Guid userId)
        {
            Model = model;
            UserId = userId;
        }
    }
}
using common.api.Commands;
using SeatQ.core.dal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SeatQ.core.api.Commands.UpdateConfirmed
{
    public class UpdateConfirmedRequest : ICommandRequest
    {
        public WaitListIdModel Model { get; set; }

        public UpdateConfirmedRequest(WaitListIdModel model)
        {
            Model = model;
        }
    }
}
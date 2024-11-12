using common.api.Commands;
using SeatQ.core.dal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SeatQ.core.api.Commands.UpdateLeave
{
    public class UpdateLeaveRequest : ICommandRequest
    {
        public WaitListIdModel Model { get; set; }

        public UpdateLeaveRequest(WaitListIdModel model)
        {
            Model = model;
        }
    }
}
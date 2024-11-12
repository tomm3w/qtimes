using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using common.api.Commands;
using SeatQ.core.dal.Models;

namespace SeatQ.core.api.Commands.AddVisitMessage
{
    public class AddVisitMessageRequest : ICommandRequest
    {
        public VisitMessage Model { get; private set; }
        public AddVisitMessageRequest(VisitMessage model)
		{
            Model = model;
		}
    }
}
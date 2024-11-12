using common.api.Commands;
using SeatQ.core.dal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SeatQ.core.api.Commands.EditMessages
{
    public class EditMessagesRequest : ICommandRequest
    {
        public AllMessagesModel Model { get; private set; }

        public EditMessagesRequest(AllMessagesModel model)
        {
            Model = model;
        }
    }
}
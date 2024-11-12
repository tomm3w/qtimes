using common.api.Commands;
using SeatQ.core.dal.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SeatQ.core.api.Commands.EditHostess
{
    public class EditHostessRequest : ICommandRequest
    {
        public HostessModel Model { get; private set; }
        public EditHostessRequest(HostessModel model)
		{
            Model = model;
		}
    }
}
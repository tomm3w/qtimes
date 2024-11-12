using common.api.Commands;
using Core.Attributes;
using SeatQ.core.dal.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SeatQ.core.api.Commands.EditRestaurantChain
{
    public class EditRestaurantChainRequest : ICommandRequest
    {

        public RestaurantInfoModel Model { get; private set; }
       

        public EditRestaurantChainRequest(RestaurantInfoModel model)
        {
            Model = model;
        }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SeatQ.core.dal.Models
{
    public class RestaurantChainIdModel
    {
        [Required]
        public int RestaurantChainId { get; set; }
    }
}
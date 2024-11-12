using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SeatQ.core.dal.Models
{
    public class MetricsModel
    {
        [Required]
        public int RestaurantChainId { get; set; }

        [Required]
        public string MetricsType { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }
    }
}
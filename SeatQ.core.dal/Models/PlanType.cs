//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SeatQ.core.dal.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class PlanType
    {
        public PlanType()
        {
            this.Restaurants = new HashSet<Restaurant>();
        }
    
        public int PlanId { get; set; }
        public string PlanType1 { get; set; }
        public string Description { get; set; }
    
        public virtual ICollection<Restaurant> Restaurants { get; set; }
    }
}

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
    
    public partial class VisitMessage
    {
        public int VisitMessageId { get; set; }
        public Nullable<int> RestaurantChainId { get; set; }
        public int Visit { get; set; }
        public string VisitMessage1 { get; set; }
        public Nullable<bool> IsEnabled { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
    
        public virtual RestaurantChain RestaurantChain { get; set; }
    }
}

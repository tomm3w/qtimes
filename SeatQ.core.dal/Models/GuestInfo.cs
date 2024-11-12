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
    
    public partial class GuestInfo
    {
        public GuestInfo()
        {
            this.LoyaltyMessages = new HashSet<LoyaltyMessage>();
            this.VisitMessageSents = new HashSet<VisitMessageSent>();
            this.WaitLists = new HashSet<WaitList>();
        }
    
        public long GuestId { get; set; }
        public string MobileNumber { get; set; }
        public Nullable<int> NoOfReturn { get; set; }
        public Nullable<int> RestaurantChainId { get; set; }
        public string GuestName { get; set; }
    
        public virtual RestaurantChain RestaurantChain { get; set; }
        public virtual ICollection<LoyaltyMessage> LoyaltyMessages { get; set; }
        public virtual ICollection<VisitMessageSent> VisitMessageSents { get; set; }
        public virtual ICollection<WaitList> WaitLists { get; set; }
    }
}

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
    
    public partial class UsersInRestaurant
    {
        public int UsersInRestaurantId { get; set; }
        public Nullable<System.Guid> UserId { get; set; }
        public Nullable<int> RestaurantChainId { get; set; }
        public Nullable<int> BusinessId { get; set; }
    
        public virtual UserInfo UserInfo { get; set; }
        public virtual RestaurantChain RestaurantChain { get; set; }
    }
}

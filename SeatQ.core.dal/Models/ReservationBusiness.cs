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
    
    public partial class ReservationBusiness
    {
        public ReservationBusiness()
        {
            this.BusinessDetails = new HashSet<BusinessDetail>();
            this.UserInfoes = new HashSet<UserInfo>();
            this.Concerts = new HashSet<Concert>();
        }
    
        public System.Guid Id { get; set; }
        public string BusinessName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string TimezoneOffset { get; set; }
        public string TimezoneOffsetValue { get; set; }
    
        public virtual ICollection<BusinessDetail> BusinessDetails { get; set; }
        public virtual ICollection<UserInfo> UserInfoes { get; set; }
        public virtual ICollection<Concert> Concerts { get; set; }
    }
}

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
    
    public partial class GuestType
    {
        public GuestType()
        {
            this.WaitLists = new HashSet<WaitList>();
            this.ConcertReservations = new HashSet<ConcertReservation>();
            this.Reservations = new HashSet<Reservation>();
        }
    
        public int GuestTypeId { get; set; }
        public string GuestType1 { get; set; }
    
        public virtual ICollection<WaitList> WaitLists { get; set; }
        public virtual ICollection<ConcertReservation> ConcertReservations { get; set; }
        public virtual ICollection<Reservation> Reservations { get; set; }
    }
}

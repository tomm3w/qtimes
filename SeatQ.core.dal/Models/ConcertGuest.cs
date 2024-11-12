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
    
    public partial class ConcertGuest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string MobileNo { get; set; }
        public string SeatNo { get; set; }
        public Nullable<int> ConcertReservationId { get; set; }
        public string Photo { get; set; }
        public string Temperature { get; set; }
        public Nullable<System.DateTime> CheckInTime { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Email { get; set; }
        public Nullable<System.DateTime> DOB { get; set; }
        public Nullable<bool> IsMainGuest { get; set; }
        public Nullable<bool> IsSolo { get; set; }
    
        public virtual ConcertReservation ConcertReservation { get; set; }
    }
}
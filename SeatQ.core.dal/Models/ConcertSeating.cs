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
    
    public partial class ConcertSeating
    {
        public int Id { get; set; }
        public Nullable<System.Guid> ConcertId { get; set; }
        public string Name { get; set; }
        public Nullable<short> Spots { get; set; }
        public Nullable<short> SeatsPerSpot { get; set; }
        public Nullable<System.TimeSpan> CheckInTime { get; set; }
    
        public virtual Concert Concert { get; set; }
    }
}

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
    
    public partial class ReservationMessageRule
    {
        public int Id { get; set; }
        public Nullable<System.Guid> BusinessDetailId { get; set; }
        public string MessageType { get; set; }
        public Nullable<short> Value { get; set; }
        public string ValueType { get; set; }
        public string BeforeAfter { get; set; }
        public string InOut { get; set; }
        public string Message { get; set; }
    
        public virtual BusinessDetail BusinessDetail { get; set; }
    }
}

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
    
    public partial class MessageReplyType
    {
        public MessageReplyType()
        {
            this.WaitLists = new HashSet<WaitList>();
        }
    
        public byte MessageReply { get; set; }
        public string MessageReplyType1 { get; set; }
    
        public virtual ICollection<WaitList> WaitLists { get; set; }
    }
}

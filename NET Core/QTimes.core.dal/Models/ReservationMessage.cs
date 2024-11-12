using System;
using System.Collections.Generic;

namespace QTimes.core.dal.Models
{
    public partial class ReservationMessage
    {
        public int Id { get; set; }
        public int? ReservationId { get; set; }
        public string MessageSent { get; set; }
        public DateTime? MessageSentDateTime { get; set; }
        public Guid? MessageSentBy { get; set; }
        public string MessageReplied { get; set; }
        public DateTime? MessageRepliedDateTime { get; set; }
        public bool? IsRead { get; set; }

        public virtual Reservation Reservation { get; set; }
    }
}

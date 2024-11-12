using System;
using System.Collections.Generic;

namespace QTimes.core.dal.Models
{
    public partial class ConcertEventReservationMessage
    {
        public int Id { get; set; }
        public int? ConcertEventReservationId { get; set; }
        public string MessageSent { get; set; }
        public DateTime? MessageSentDateTime { get; set; }
        public Guid? MessageSentBy { get; set; }
        public string MessageReplied { get; set; }
        public DateTime? MessageRepliedDateTime { get; set; }
        public bool? IsRead { get; set; }

        public virtual ConcertEventReservation ConcertEventReservation { get; set; }
    }
}

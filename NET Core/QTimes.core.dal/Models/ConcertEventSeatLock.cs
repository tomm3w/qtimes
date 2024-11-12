using System;
using System.Collections.Generic;

namespace QTimes.core.dal.Models
{
    public partial class ConcertEventSeatLock
    {
        public int Id { get; set; }
        public string SeatNo { get; set; }
        public DateTime? LockTime { get; set; }
        public DateTime? ReleaseTime { get; set; }
        public Guid? ConcertEventId { get; set; }

        public virtual ConcertEvent ConcertEvent { get; set; }
    }
}

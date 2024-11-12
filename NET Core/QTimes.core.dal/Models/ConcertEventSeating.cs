using System;
using System.Collections.Generic;

namespace QTimes.core.dal.Models
{
    public partial class ConcertEventSeating
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public short? Spots { get; set; }
        public short? SeatsPerSpot { get; set; }
        public TimeSpan? CheckInTime { get; set; }
        public Guid? ConcertEventId { get; set; }

        public virtual ConcertEvent ConcertEvent { get; set; }
    }
}

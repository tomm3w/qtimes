using System;
using System.Collections.Generic;

namespace QTimes.core.dal.Models
{
    public partial class ConcertEventMessageRule
    {
        public int Id { get; set; }
        public string MessageType { get; set; }
        public short? Value { get; set; }
        public string ValueType { get; set; }
        public string BeforeAfter { get; set; }
        public string InOut { get; set; }
        public string Message { get; set; }
        public Guid? ConcertEventId { get; set; }

        public virtual ConcertEvent ConcertEvent { get; set; }
    }
}

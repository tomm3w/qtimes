using System;

namespace QTimes.core.dal.Models
{
    public class TimeSlot
    {
        public TimeSpan? OpenHourFrom { get; set; }
        public TimeSpan? OpenHourTo { get; set; }
        public int? SpotLeft { get; set; }
    }
}

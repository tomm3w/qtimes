using System;

namespace SeatQ.core.api.Models.Pass
{
    public sealed class QTimesPassModel
    {
        public string TemplateId { get; set; }
        public string Name { get; set; }
        public string MobileNumber { get; set; }
        public short? GroupSize { get; set; }
        public DateTime? Date { get; set; }
        public TimeSpan? TimeFrom { get; set; }
        public TimeSpan? TimeTo { get; set; }
        public string Comments { get; set; }
        public DateTime? CheckInTime { get; set; }
    }
}
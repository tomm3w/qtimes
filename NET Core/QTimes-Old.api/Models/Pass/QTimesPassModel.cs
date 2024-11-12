using System;

namespace QTimes.api.Models.Pass
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
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Address { get; set; }
        public string FirstName { get; set; }
        public string FamiliName { get; set; }
        public string DOB { get; set; }
        public string FacePhotoImageBase64 { get; set; }
    }
}
using iVeew.common.api.Commands;
using System;
using System.ComponentModel.DataAnnotations;

namespace QTimes.api.Commands.AddBusinessDetail
{
    public class AddBusinessDetailRequest : ICommandRequest
    {
        [Required]
        public string Name { get; set; }
        public Guid UserId { get; set; }
        public Guid ReservationBusinessId { get; set; }
        [Required]
        public short? BusinessTypeId { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public Nullable<System.TimeSpan> TimeFrom { get; set; }
        public Nullable<System.TimeSpan> TimeTo { get; set; }
    }
}
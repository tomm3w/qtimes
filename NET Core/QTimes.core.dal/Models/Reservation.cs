using System;
using System.Collections.Generic;

namespace QTimes.core.dal.Models
{
    public partial class Reservation
    {
        public Reservation()
        {
            ReservationMessage = new HashSet<ReservationMessage>();
        }

        public int Id { get; set; }
        public Guid? ReservationGuestId { get; set; }
        public int? GuestTypeId { get; set; }
        public short? Size { get; set; }
        public DateTime? Date { get; set; }
        public TimeSpan? TimeFrom { get; set; }
        public TimeSpan? TimeTo { get; set; }
        public DateTime? UtcDateTimeFrom { get; set; }
        public DateTime? UtcDateTimeTo { get; set; }
        public DateTime? TimeIn { get; set; }
        public DateTime? TimeOut { get; set; }
        public string Comments { get; set; }
        public bool? IsCancelled { get; set; }
        public DateTime? CancelledDateTime { get; set; }
        public DateTime? CreatedDateTime { get; set; }
        public Guid? CreatedBy { get; set; }
        public Guid? BusinessDetailId { get; set; }
        public string PassUrl { get; set; }

        public virtual BusinessDetail BusinessDetail { get; set; }
        public virtual GuestType GuestType { get; set; }
        public virtual ReservationGuest ReservationGuest { get; set; }
        public virtual ICollection<ReservationMessage> ReservationMessage { get; set; }
    }
}

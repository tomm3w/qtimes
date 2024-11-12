using System;
using System.Collections.Generic;

namespace QTimes.core.dal.Models
{
    public partial class ConcertEventReservation
    {
        public ConcertEventReservation()
        {
            ConcertEventGuest = new HashSet<ConcertEventGuest>();
            ConcertEventReservationMessage = new HashSet<ConcertEventReservationMessage>();
        }

        public int Id { get; set; }
        public short? Size { get; set; }
        public string Seatings { get; set; }
        public string ConfirmationNo { get; set; }
        public int? GuestTypeId { get; set; }
        public Guid? ConcertEventId { get; set; }
        public string PassUrl { get; set; }

        public virtual ConcertEvent ConcertEvent { get; set; }
        public virtual GuestType GuestType { get; set; }
        public virtual ICollection<ConcertEventGuest> ConcertEventGuest { get; set; }
        public virtual ICollection<ConcertEventReservationMessage> ConcertEventReservationMessage { get; set; }
    }
}

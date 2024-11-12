using System;
using System.Collections.Generic;

namespace QTimes.core.dal.Models
{
    public partial class GuestType
    {
        public GuestType()
        {
            ConcertEventReservation = new HashSet<ConcertEventReservation>();
            Reservation = new HashSet<Reservation>();
        }

        public int GuestTypeId { get; set; }
        public string GuestType1 { get; set; }

        public virtual ICollection<ConcertEventReservation> ConcertEventReservation { get; set; }
        public virtual ICollection<Reservation> Reservation { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace QTimes.core.dal.Models
{
    public partial class ConcertEventGuest
    {
        public int Id { get; set; }
        public int? ConcertEventReservationId { get; set; }
        public string Name { get; set; }
        public string MobileNo { get; set; }
        public string SeatNo { get; set; }
        public string Photo { get; set; }
        public string Temperature { get; set; }
        public DateTime? CheckInTime { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Email { get; set; }
        public DateTime? Dob { get; set; }
        public bool? IsMainGuest { get; set; }
        public bool? IsSolo { get; set; }
        public string FamilyName { get; set; }
        public string FirstName { get; set; }
        public string FacePhotoImageBase64 { get; set; }

        public virtual ConcertEventReservation ConcertEventReservation { get; set; }
    }
}

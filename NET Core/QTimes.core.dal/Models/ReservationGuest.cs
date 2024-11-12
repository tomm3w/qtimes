using System;
using System.Collections.Generic;

namespace QTimes.core.dal.Models
{
    public partial class ReservationGuest
    {
        public ReservationGuest()
        {
            Reservation = new HashSet<Reservation>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public Guid? BusinessDetailId { get; set; }
        public DateTime? Dob { get; set; }
        public string FamilyName { get; set; }
        public string FirstName { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Zip { get; set; }
        public string Address { get; set; }
        public string FacePhotoImageBase64 { get; set; }

        public virtual BusinessDetail BusinessDetail { get; set; }
        public virtual ICollection<Reservation> Reservation { get; set; }
    }
}

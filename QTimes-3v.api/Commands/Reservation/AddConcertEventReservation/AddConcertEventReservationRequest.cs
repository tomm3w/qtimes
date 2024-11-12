using iVeew.common.api.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QTimes.api.Commands.Reservation.AddConcertEventReservation
{
    public class AddConcertEventReservationRequest : IAsyncCommandRequest
    {
        [Required]
        public Nullable<short> Size { get; set; }

        public string Seatings { get; set; }
        public List<Guest> Guests { get; set; }
        public Nullable<int> GuestTypeId { get; set; }
        [Required]
        public string MainGuestName { get; set; }
        [Required]
        public string MainGuestMobileNo { get; set; }
        [EmailAddress]
        public string MainGuestEmail { get; set; }
        [Required]
        public string MainGuestSeatNo { get; set; }
        [Required]
        public string MainGuestSeatName { get; set; }
        [Required]
        public Guid ConcertEventId { get; set; }
        public DateTime? Dob { get; set; }
        public string FamilyName { get; set; }
        public string FirstName { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Zip { get; set; }
        public string Address { get; set; }
        public string FacePhotoImageBase64 { get; set; }
    }

    public class Guest
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string MobileNo { get; set; }
        [Required]
        public string SeatNo { get; set; }
        [Required]
        public string SeatName { get; set; }
    }
}
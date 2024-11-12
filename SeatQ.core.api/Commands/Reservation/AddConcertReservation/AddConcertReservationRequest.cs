using common.api.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SeatQ.core.api.Commands.Reservation.AddConcertReservation
{
    public class AddConcertReservationRequest : ICommandRequest
    {
        [Required]
        public Nullable<System.Guid> ConcertId { get; set; }
        [Required]
        public Nullable<short> Size { get; set; }

        public string Seatings { get; set; }

        public Nullable<System.DateTime> ReservationDate { get; set; }
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
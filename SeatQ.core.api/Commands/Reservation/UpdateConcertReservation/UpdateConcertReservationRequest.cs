using common.api.Commands;
using SeatQ.core.api.Commands.Reservation.AddConcertReservation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SeatQ.core.api.Commands.Reservation.UpdateConcertReservation
{
    public class UpdateConcertReservationRequest : ICommandRequest
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public Nullable<System.Guid> ConcertId { get; set; }
        [Required]
        public Nullable<short> Size { get; set; }
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
}
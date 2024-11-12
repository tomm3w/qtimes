using iVeew.common.api.Commands;
using QTimes.api.Commands.Reservation.AddConcertEventReservation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QTimes.api.Commands.Reservation.UpdateConcertEventReservation
{
    public class UpdateConcertEventReservationRequest : ICommandRequest
    {
        //Event Reservation Id
        [Required]
        public int Id { get; set; }
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
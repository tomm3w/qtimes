using common.api.Commands;
using System;
using System.ComponentModel.DataAnnotations;

namespace SeatQ.core.api.Commands.Reservation.CancelReservation
{
    public class CancelReservationRequest : ICommandRequest
    {
        [Required]
        public int? Id { get; set; }
        public Guid UserId { get; set; }
        public Nullable<System.Guid> BusinessDetailId { get; set; }
    }
}
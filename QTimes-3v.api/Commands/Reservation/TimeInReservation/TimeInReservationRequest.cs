using iVeew.common.api.Commands;
using System;
using System.ComponentModel.DataAnnotations;

namespace QTimes.api.Commands.Reservation.TimeInReservation
{
    public class TimeInReservationRequest : ICommandRequest
    {
        [Required]
        public int? Id { get; set; }
        public Nullable<System.Guid> BusinessDetailId { get; set; }
        public Guid UserId { get; set; }
    }
}
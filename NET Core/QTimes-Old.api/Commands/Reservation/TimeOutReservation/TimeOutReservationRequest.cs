using iVeew.common.api.Commands;
using System;
using System.ComponentModel.DataAnnotations;

namespace QTimes.api.Commands.Reservation.TimeOutReservation
{
    public class TimeOutReservationRequest : ICommandRequest
    {
        [Required]
        public int? Id { get; set; }
        public Nullable<System.Guid> BusinessDetailId { get; set; }
        public Guid UserId { get; set; }
    }
}
using common.api.Commands;
using System;
using System.ComponentModel.DataAnnotations;

namespace SeatQ.core.api.Commands.Reservation.EditReservation
{
    public class EditReservationRequest : ICommandRequest
    {
        public Guid UserId { get; set; }
        [Required]
        public int? Id { get; set; }
        [Required]
        public Nullable<System.Guid> ReservationGuestId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string MobileNo { get; set; }
        public Nullable<System.Guid> BusinessDetailId { get; set; }
        [Required]
        public Nullable<int> GuestTypeId { get; set; }
        [Required]
        public Nullable<short> Size { get; set; }
        [Required]
        public Nullable<System.DateTime> Date { get; set; }
        [Required]
        public Nullable<System.TimeSpan> TimeFrom { get; set; }
        [Required]
        public Nullable<System.TimeSpan> TimeTo { get; set; }
        public string Comments { get; set; }
    }
}
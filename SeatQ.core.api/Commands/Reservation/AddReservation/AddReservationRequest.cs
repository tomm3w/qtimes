using common.api.Commands;
using System;
using System.ComponentModel.DataAnnotations;

namespace SeatQ.core.api.Commands.Reservation.AddReservation
{
    public class AddReservationRequest : IAsyncCommandRequest<string>
    {
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
        public Guid CreatedBy { get; set; }

    }
}
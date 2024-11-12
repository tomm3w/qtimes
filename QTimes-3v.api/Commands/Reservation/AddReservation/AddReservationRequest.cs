using iVeew.common.api.Commands;
using System;
using System.ComponentModel.DataAnnotations;

namespace QTimes.api.Commands.Reservation.AddReservation
{
    public class AddReservationRequest : IAsyncCommandRequest
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
        public DateTime? Dob { get; set; }
        public string FamilyName { get; set; }
        public string FirstName { get; set; }
        public string State{ get; set; }
        public string City{ get; set; }
        public string Zip{ get; set; }
        public string Address{ get; set; }
        public string FacePhotoImageBase64 { get; set; }
        public string Email { get; set; }

    }
}
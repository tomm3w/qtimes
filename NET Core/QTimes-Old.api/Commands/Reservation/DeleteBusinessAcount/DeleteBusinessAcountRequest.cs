using iVeew.common.api.Commands;
using QTimes.api.Models.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace QTimes.api.Commands.Reservation.DeleteBusinessAcount
{
    public class DeleteBusinessAcountRequest : ICommandRequest
    {
        public Guid UserId { get; set; }
        [Required]
        public Guid Id { get; set; }
        [Required]
        public Guid ReservationBusinessId { get; set; }
        public BusinessTypeEnum BusinessType { get; set; }
    }
}

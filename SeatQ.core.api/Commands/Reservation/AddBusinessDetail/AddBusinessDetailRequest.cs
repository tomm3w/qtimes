using common.api.Commands;
using System;
using System.ComponentModel.DataAnnotations;

namespace SeatQ.core.api.Commands.AddBusinessDetail
{
    public class AddBusinessDetailRequest : ICommandRequest
    {
        [Required]
        public string Name { get; set; }
        public Guid UserId { get; set; }
        public Guid ReservationBusinessId { get; set; }
        [Required]
        public short? BusinessTypeId { get; set; }
    }
}
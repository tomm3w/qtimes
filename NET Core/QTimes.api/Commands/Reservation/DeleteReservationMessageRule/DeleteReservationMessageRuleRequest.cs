using iVeew.common.api.Commands;
using System.ComponentModel.DataAnnotations;

namespace QTimes.api.Commands.Reservation.DeleteReservationMessageRule
{
    public class DeleteReservationMessageRuleRequest : ICommandRequest
    {
        [Required]
        public int Id { get; set; }
    }
}
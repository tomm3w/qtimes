using common.api.Commands;
using System;

namespace SeatQ.core.api.Commands.Reservation.LockConcertSeat
{
    public class LockConcertSeatRequest : ICommandRequest
    {
        public Nullable<System.Guid> ConcertId { get; set; }
        public string SeatNo { get; set; }
    }
}
using iVeew.common.api.Commands;
using System;

namespace QTimes.api.Commands.Reservation.LockConcertEventSeat
{
    public class LockConcertEventSeatRequest : ICommandRequest
    {
        public Nullable<System.Guid> ConcertEventId { get; set; }
        public string SeatNo { get; set; }
    }
}
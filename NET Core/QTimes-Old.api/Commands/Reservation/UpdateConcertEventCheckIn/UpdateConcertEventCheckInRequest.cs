using iVeew.common.api.Commands;

namespace QTimes.api.Commands.Reservation.UpdateConcertEventCheckIn
{
    public class UpdateConcertEventCheckInRequest : ICommandRequest
    {
        public int ConcertGuestId { get; set; }
        public int ConcertEventReservationId { get; set; }
        public string Temperature { get; set; }
        public bool IsCheckedIn { get; set; }
        public bool IsSolo { get; set; }
    }
}
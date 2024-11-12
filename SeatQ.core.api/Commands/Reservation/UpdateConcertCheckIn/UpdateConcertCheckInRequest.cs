using common.api.Commands;

namespace SeatQ.core.api.Commands.Reservation.UpdateConcertCheckIn
{
    public class UpdateConcertCheckInRequest : ICommandRequest
    {
        public int ConcertGuestId { get; set; }
        public int ConcertReservationId { get; set; }
        public string Temperature { get; set; }
        public bool IsCheckedIn { get; set; }
        public bool IsSolo { get; set; }
    }
}
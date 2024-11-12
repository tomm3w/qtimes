using iVeew.common.api.Commands;
using IVeew.common.dal;
using QTimes.api.Exceptions;
using QTimes.core.dal.Models;
using System;
using System.Linq;

namespace QTimes.api.Commands.Reservation.UpdateConcertEventCheckIn
{
    public class UpdateConcertEventCheckInCommand : ICommand<UpdateConcertEventCheckInRequest>
    {
        private readonly IGenericRepository<QTimesContext, ConcertEventGuest> _concertRepository;
        public UpdateConcertEventCheckInCommand(IGenericRepository<QTimesContext, ConcertEventGuest> concertRepository)
        {
            _concertRepository = concertRepository;
        }
        public void Handle(UpdateConcertEventCheckInRequest request)
        {
            var concert = _concertRepository.FindBy(x => x.Id == request.ConcertGuestId && x.ConcertEventReservationId == request.ConcertEventReservationId).FirstOrDefault();
            if (concert == null)
                throw new NotFoundException();

            concert.Temperature = request.Temperature;
            concert.IsSolo = request.IsSolo;
            if (request.IsCheckedIn)
                concert.CheckInTime = DateTime.UtcNow;
            else
                concert.CheckInTime = null;

            _concertRepository.Save();
        }
    }
}
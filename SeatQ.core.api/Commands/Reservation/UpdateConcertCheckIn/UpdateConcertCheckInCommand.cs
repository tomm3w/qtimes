using common.api.Commands;
using common.dal;
using SeatQ.core.api.Exceptions;
using SeatQ.core.dal.Models;
using System;
using System.Linq;

namespace SeatQ.core.api.Commands.Reservation.UpdateConcertCheckIn
{
    public class UpdateConcertCheckInCommand : ICommand<UpdateConcertCheckInRequest>
    {
        private readonly IGenericRepository<SeatQEntities, ConcertGuest> _concertRepository;
        public UpdateConcertCheckInCommand(IGenericRepository<SeatQEntities, ConcertGuest> concertRepository)
        {
            _concertRepository = concertRepository;
        }
        public void Handle(UpdateConcertCheckInRequest request)
        {
            var concert = _concertRepository.FindBy(x => x.Id == request.ConcertGuestId && x.ConcertReservationId == request.ConcertReservationId).FirstOrDefault();
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
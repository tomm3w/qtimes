using common.api.Commands;
using common.dal;
using SeatQ.core.dal.Models;
using System;
using System.Linq;

namespace SeatQ.core.api.Commands.Reservation.LockConcertSeat
{

    public class LockConcertSeatCommand : ICommand<LockConcertSeatRequest>
    {
        private readonly IGenericRepository<SeatQEntities, Concert> _concertRepository;
        private readonly IGenericRepository<SeatQEntities, ConcertGuest> _concertGuestRepository;
        private readonly IGenericRepository<SeatQEntities, ConcertSeatLock> _concertSeatLockRepository;
        public LockConcertSeatCommand(IGenericRepository<SeatQEntities, ConcertSeatLock> concertSeatLockRepository,
            IGenericRepository<SeatQEntities, Concert> concertRepository,
            IGenericRepository<SeatQEntities, ConcertGuest> concertGuestRepository)
        {
            _concertSeatLockRepository = concertSeatLockRepository;
            _concertRepository = concertRepository;
            _concertGuestRepository = concertGuestRepository;
        }

        public void Handle(LockConcertSeatRequest request)
        {
            var concert = _concertRepository.FindBy(x => x.Id == request.ConcertId).FirstOrDefault();

            if (concert != null && concert.EnableTimeExpiration == true)
            {
                var reserved = _concertGuestRepository.FindBy(x => x.ConcertReservation.ConcertId == request.ConcertId && x.SeatNo == request.SeatNo);
                if (reserved.Any())
                    throw new Exception($"{request.SeatNo} already taken.");

                var reserve = _concertSeatLockRepository.FindBy(x => x.ConcertId == request.ConcertId && x.SeatNo == request.SeatNo && x.ReleaseTime >= DateTime.UtcNow);
                if (reserve.Any())
                    throw new Exception($"{request.SeatNo} locked for reservation.");

                var newReserve = new ConcertSeatLock
                {
                    ConcertId = request.ConcertId,
                    SeatNo = request.SeatNo,
                    LockTime = DateTime.UtcNow,
                    ReleaseTime = DateTime.UtcNow.AddMinutes(concert.TimeExpiration ?? 0)
                };

                _concertSeatLockRepository.Add(newReserve);
                _concertSeatLockRepository.Save();
            }
        }
    }
}
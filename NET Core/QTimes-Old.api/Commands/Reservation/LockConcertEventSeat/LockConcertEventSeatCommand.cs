using iVeew.common.api.Commands;
using IVeew.common.dal;
using QTimes.core.dal.Models;
using System;
using System.Linq;

namespace QTimes.api.Commands.Reservation.LockConcertEventSeat
{

    public class LockConcertEventSeatCommand : ICommand<LockConcertEventSeatRequest>
    {
        private readonly IGenericRepository<QTimesContext, ConcertEvent> _eventRepository;
        private readonly IGenericRepository<QTimesContext, ConcertEventGuest> _concertGuestRepository;
        private readonly IGenericRepository<QTimesContext, ConcertEventSeatLock> _concertSeatLockRepository;
        public LockConcertEventSeatCommand(IGenericRepository<QTimesContext, ConcertEventSeatLock> concertSeatLockRepository,
            IGenericRepository<QTimesContext, ConcertEvent> eventRepository,
            IGenericRepository<QTimesContext, ConcertEventGuest> concertGuestRepository)
        {
            _concertSeatLockRepository = concertSeatLockRepository;
            _eventRepository = eventRepository;
            _concertGuestRepository = concertGuestRepository;
        }

        public void Handle(LockConcertEventSeatRequest request)
        {
            var @event = _eventRepository.FindBy(x => x.Id == request.ConcertEventId).FirstOrDefault();

            if (@event != null && @event.EnableTimeExpiration == true)
            {
                var reserved = _concertGuestRepository.FindBy(x => x.ConcertEventReservation.ConcertEventId == request.ConcertEventId && x.SeatNo.ToLower() == request.SeatNo.ToLower());
                if (reserved.Any())
                    throw new Exception($"{request.SeatNo} already taken.");

                var reserve = _concertSeatLockRepository.FindBy(x => x.ConcertEventId == request.ConcertEventId && x.SeatNo.ToLower() == request.SeatNo.ToLower() && x.ReleaseTime >= DateTime.UtcNow);
                if (reserve.Any())
                    throw new Exception($"{request.SeatNo} locked for reservation.");

                var newReserve = new ConcertEventSeatLock
                {
                    ConcertEventId = request.ConcertEventId,
                    SeatNo = request.SeatNo,
                    LockTime = DateTime.UtcNow,
                    ReleaseTime = DateTime.UtcNow.AddMinutes(@event.TimeExpiration ?? 0)
                };

                _concertSeatLockRepository.Add(newReserve);
                _concertSeatLockRepository.Save();
            }
        }
    }
}
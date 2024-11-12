using iVeew.common.api.Commands;
using IVeew.common.dal;
using QTimes.api.Exceptions;
using QTimes.core.dal.Models;
using System.Linq;

namespace QTimes.api.Commands.Reservation.AddConcertEventSeating
{
    public class AddConcertEventSeatingCommand : ICommand<AddConcertEventSeatingRequest>
    {
        private readonly IGenericRepository<QTimesContext, ConcertEvent> _eventRepository;
        private readonly IGenericRepository<QTimesContext, ConcertEventSeating> _concertSeatingRepository;

        public AddConcertEventSeatingCommand(IGenericRepository<QTimesContext, ConcertEvent> eventRepository,
            IGenericRepository<QTimesContext, ConcertEventSeating> concertSeatingRepository)
        {
            _eventRepository = eventRepository;
            _concertSeatingRepository = concertSeatingRepository;
        }

        public void Handle(AddConcertEventSeatingRequest request)
        {
            var @event = _eventRepository.FindBy(x => x.Id == request.EventId).FirstOrDefault();

            if (@event == null)
                throw new NotFoundException("Event not found");

            ValidateSeatings(@event, request);

            @event.SeatMapPath = request.SeatMapPath;
            @event.SeatType = request.SeatType;
            @event.AllowCheckInTime = request.AllowCheckInTime;

            var seatings = _concertSeatingRepository.FindBy(x => x.ConcertEventId == request.EventId);

            if (seatings.Any())
            {
                seatings.ToList().ForEach(item =>
                    _concertSeatingRepository.Delete(item)
                );
                _concertSeatingRepository.Save();
            }

            if (request.ConcertSeatings != null && request.ConcertSeatings.Any())
            {

                foreach (var item in request.ConcertSeatings)
                {
                    var seat = new ConcertEventSeating()
                    {
                        SeatsPerSpot = item.SeatsPerSpot,
                        Name = item.Name,
                        Spots = item.Spots,
                        CheckInTime = item.CheckInTime
                    };

                    @event.ConcertEventSeating.Add(seat);

                }

            }

            _eventRepository.Save();
        }

        private void ValidateSeatings(ConcertEvent concertEvent, AddConcertEventSeatingRequest request)
        {
            if (concertEvent.EnableTotalCapacity ?? false)
            {
                var totalrequestedSeatings = request.ConcertSeatings.Sum(x => (x.SeatsPerSpot ?? 0) * (x.Spots ?? 0));
                if (concertEvent.TotalCapacity < totalrequestedSeatings)
                    throw new InvalidReservationDataException("Total seatings capacity exceeded.");
            }
        }

    }
}
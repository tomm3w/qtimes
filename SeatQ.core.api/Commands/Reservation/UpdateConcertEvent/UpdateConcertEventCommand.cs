using common.api.Commands;
using common.dal;
using SeatQ.core.api.Exceptions;
using SeatQ.core.dal.Models;
using System.Linq;

namespace SeatQ.core.api.Commands.Reservation.UpdateConcertEvent
{
    public class UpdateConcertEventCommand : ICommand<UpdateConcertEventRequest>
    {
        private readonly IGenericRepository<SeatQEntities, Concert> _concertRepository;
        public UpdateConcertEventCommand(IGenericRepository<SeatQEntities, Concert> concertRepository)
        {
            _concertRepository = concertRepository;
        }
        public void Handle(UpdateConcertEventRequest request)
        {
            var concert = _concertRepository.FindBy(x => x.Id == request.Id).FirstOrDefault();
            if (concert == null)
                throw new NotFoundException();

            concert.Name = request.Name;
            concert.Date = request.Date;
            concert.TimeFrom = request.TimeFrom;
            concert.TimeTo = request.TimeTo;
            _concertRepository.Save();
        }
    }
}
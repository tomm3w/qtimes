using iVeew.common.api.Commands;
using IVeew.common.dal;
using QTimes.api.Exceptions;
using QTimes.core.dal.Models;
using System.Linq;

namespace QTimes.api.Commands.Reservation.UpdateConcertEvent
{
    public class UpdateConcertEventCommand : ICommand<UpdateConcertEventRequest>
    {
        private readonly IGenericRepository<QTimesContext, ConcertEvent> _concertEventRepository;
        public UpdateConcertEventCommand(IGenericRepository<QTimesContext, ConcertEvent> concertEventRepository)
        {
            _concertEventRepository = concertEventRepository;
        }
        public void Handle(UpdateConcertEventRequest request)
        {
            var concertEvent = _concertEventRepository.FindBy(x => x.Id == request.Id).FirstOrDefault();
            if (concertEvent == null)
                throw new NotFoundException();

            concertEvent.Name = request.Name;
            concertEvent.Date = request.Date;
            concertEvent.TimeFrom = request.TimeFrom;
            concertEvent.TimeTo = request.TimeTo;
            _concertEventRepository.Save();
        }
    }
}
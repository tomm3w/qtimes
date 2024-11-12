using iVeew.common.api.Commands;
using IVeew.common.dal;
using QTimes.api.Exceptions;
using QTimes.core.dal.Models;
using System;
using System.Linq;

namespace QTimes.api.Commands.Reservation.DeleteConcertEvent
{
    public class DeleteConcertEventCommand : ICommand<DeleteConcertEventRequest>
    {
        private readonly IGenericRepository<QTimesContext, ConcertEvent> _concertEventRepository;
        public DeleteConcertEventCommand(IGenericRepository<QTimesContext, ConcertEvent> concertEventRepository)
        {
            _concertEventRepository = concertEventRepository;
        }
        public void Handle(DeleteConcertEventRequest request)
        {
            var concertEvent = _concertEventRepository.FindBy(x => x.Id == request.Id).FirstOrDefault();
            if (concertEvent == null)
                throw new NotFoundException();

            concertEvent.DeletedDate = DateTime.UtcNow;
            _concertEventRepository.Save();
        }
    }
}
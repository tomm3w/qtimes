using iVeew.common.api.Commands;
using IVeew.common.dal;
using QTimes.api.Exceptions;
using QTimes.core.dal.Models;
using System.Linq;

namespace QTimes.api.Commands.Reservation.UpdateConcertEventPreference
{
    public class UpdateConcertEventPreferenceCommand : ICommand<UpdateConcertEventPreferenceRequest>
    {
        private readonly IGenericRepository<QTimesContext, ConcertEvent> _eventRepository;

        public UpdateConcertEventPreferenceCommand(IGenericRepository<QTimesContext, ConcertEvent> eventRepository)
        {
            _eventRepository = eventRepository;
        }

        public void Handle(UpdateConcertEventPreferenceRequest request)
        {

            var @event = _eventRepository.FindBy(x => x.Id == request.Id && x.ConcertId == request.ConcertId).FirstOrDefault();

            if (@event == null)
                throw new NotFoundException("Event not found");

            @event.EnableTotalCapacity = request.EnableTotalCapacity;
            @event.TotalCapacity = request.TotalCapacity;
            @event.EnableGroupSize = request.EnableGroupSize;
            @event.MinGroupSize = request.MinGroupSize;
            @event.MaxGroupSize = request.MaxGroupSize;
            @event.EnableTimeExpiration = request.EnableTimeExpiration;
            @event.TimeExpiration = request.TimeExpiration;
            @event.IsDlscanEnabled = request.IsDLScanEnabled;
            @event.IsChargeEnabled = request.IsChargeEnabled;
            @event.IsSeatmapEnabled = request.IsSeatmapEnabled;

            _eventRepository.Save();
        }
    }
}
using iVeew.common.api.Queries;
using IVeew.common.dal;
using QTimes.core.dal.Models;
using System;
using System.Linq;

namespace QTimes.api.Queries.Reservation.GetConcertMetrics
{
    public class GetMetricsQuery : IQuery<GetConcertMetricsResponse, GetConcertMetricsRequest>
    {
        private readonly IGenericRepository<QTimesContext, ConcertEventReservation> _eventReservationRepository;
        private readonly IGenericRepository<QTimesContext, ConcertEvent> _eventRepository;

        public GetMetricsQuery(IGenericRepository<QTimesContext, ConcertEventReservation> eventReservationRepository,
            IGenericRepository<QTimesContext, ConcertEvent> eventRepository)
        {
            _eventReservationRepository = eventReservationRepository;
            _eventRepository = eventRepository;
        }

        public GetConcertMetricsResponse Handle(GetConcertMetricsRequest request)
        {
            if (request.Date == null) request.Date = DateTime.UtcNow.Date;
            IQueryable<ConcertEventReservation> query;
            if (request.ConcertEventId.HasValue)
            {
                query = _eventReservationRepository.FindBy(x => x.ConcertEventId == request.ConcertEventId /* && x.ConcertEvent.Date == request.Date*/);
            }
            else
            {
                query = _eventReservationRepository.FindBy(x => x.ConcertEvent.ConcertId == request.ConcertId /*&& x.ConcertEvent.Date == request.Date*/);
            }

            var totalEvents = _eventRepository.FindBy(x => x.ConcertId == request.ConcertId).Count();
            var result = new GetConcertMetricsResponse
            {
                TotalGuests = query.Sum(x => x.Size ?? 0),
                TotalEvents = totalEvents,
                TotalReservations = query.Count() == 0 ? 0 : query.Count()
            };

            return result;
        }
    }
}
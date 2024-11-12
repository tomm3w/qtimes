using iVeew.common.api.Queries;
using IVeew.common.dal;
using QTimes.core.dal.Models;
using System.Linq;

namespace QTimes.api.Queries.Reservation.GetConcertEventSummary
{
    public class GetConcertSummaryQuery : IQuery<GetConcertEventSummaryResponse, GetConcertEventSummaryRequest>
    {
        private readonly IGenericRepository<QTimesContext, ConcertEvent> _eventRepository;
        private readonly IGenericRepository<QTimesContext, ConcertEventGuest> _guestRepository;

        public GetConcertSummaryQuery(IGenericRepository<QTimesContext, ConcertEvent> eventRepository,
            IGenericRepository<QTimesContext, ConcertEventGuest> guestRepository)
        {
            _eventRepository = eventRepository;
            _guestRepository = guestRepository;
        }

        public GetConcertEventSummaryResponse Handle(GetConcertEventSummaryRequest request)
        {
            ConcertEvent query;
            if (request.EventId.HasValue)
            {
                query = _eventRepository.FindBy(x => x.Id == request.EventId).FirstOrDefault();
            }
            else
            {
                query = _eventRepository.FindBy(x => x.ConcertId == request.ConcertId).FirstOrDefault();
            }

            var guests = _guestRepository.FindBy(x => x.ConcertEventReservation.ConcertEventId == request.EventId);

            if (query != null)
            {
                var result = new GetConcertEventSummaryResponse
                {
                    BusinessName = query.Name,
                    TotalSpots = (int)(query.TotalCapacity ?? 0),
                    Available = (int)(query.TotalCapacity ?? 0) - (int)query.ConcertEventReservation.Sum(x => (x.Size ?? 0)),
                    Waiting = guests.Count(x => x.CheckInTime == null),
                    InPlace = guests.Count(x => x.CheckInTime != null),
                    EnableGroupSize = query.EnableGroupSize ?? false,
                    MaxGroupSize = query.MaxGroupSize ?? 0,
                    ConcertEventSeatingCount = query.ConcertEventSeating.Count
                };
                return result;
            }
            else
                return new GetConcertEventSummaryResponse();

        }
    }
}
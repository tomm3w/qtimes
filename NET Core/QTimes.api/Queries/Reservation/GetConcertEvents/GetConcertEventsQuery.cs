using AutoMapper;
using iVeew.common.api.Queries;
using IVeew.common.dal;
using QTimes.api.Models.Constants;
using QTimes.core.dal.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QTimes.api.Queries.Reservation.GetConcertEvents
{
    public class GetConcertEventsQuery : IQuery<GetConcertEventsResponse, GetConcertEventsRequest>
    {
        private readonly IGenericRepository<QTimesContext, ConcertEvent> _concertEventRepository;
        public GetConcertEventsQuery(IGenericRepository<QTimesContext, ConcertEvent> concertEventRepository)
        {
            _concertEventRepository = concertEventRepository;
        }
        public GetConcertEventsResponse Handle(GetConcertEventsRequest request)
        {

            var events = _concertEventRepository.FindBy(x => x.ConcertId == request.ConcertId && x.DeletedDate == null && x.Concert.DeletedDate == null);

            if (request.EventDate != null && request.EventDate != default(DateTime))
                events = events.Where(x => x.Date == request.EventDate);

            if (!string.IsNullOrEmpty(request.SearchText))
                events = events.Where(x => x.Name.Contains(request.SearchText) || x.Description.Contains(request.SearchText));

            if (request.FilterBy == FilterByConst.Today)
                events = events.Where(x => x.Date == DateTime.UtcNow.Date);
            else if (request.FilterBy == FilterByConst.Upcoming)
                events = events.Where(x => x.Date >= DateTime.UtcNow.Date);
            else if (request.FilterBy == FilterByConst.Past)
                events = events.Where(x => x.Date < DateTime.UtcNow.Date);
            else
            {
                //All
            }

            var data = Mapper.Map<List<ConcertEventItem>>(events);
            return new GetConcertEventsResponse { Model = data.OrderByDescending(x => x.EventDate).ToList() };
        }
    }
}
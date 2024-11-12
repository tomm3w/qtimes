using AutoMapper;
using common.api.Queries;
using common.dal;
using SeatQ.core.api.Exceptions;
using SeatQ.core.dal.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SeatQ.core.api.Queries.Reservation.GetConcertEvents
{
    public class GetConcertEventsQuery : IQuery<GetConcertEventsResponse, GetConcertEventsRequest>
    {
        private readonly IGenericRepository<SeatQEntities, Concert> _concertRepository;
        private readonly IGenericRepository<SeatQEntities, UserInfo> _userInfoRepository;
        public GetConcertEventsQuery(IGenericRepository<SeatQEntities, Concert> concertRepository,
            IGenericRepository<SeatQEntities, UserInfo> userInfoRepository)
        {
            _concertRepository = concertRepository;
            _userInfoRepository = userInfoRepository;
        }
        public GetConcertEventsResponse Handle(GetConcertEventsRequest request)
        {
            if (request.ReservationBusinessId == null || request.ReservationBusinessId == default(Guid))
            {
                var user = _userInfoRepository.FindBy(x => x.UserId == request.UserId).FirstOrDefault();
                if (user.ReservationBusinessId == null)
                    throw new NotFoundException();

                request.ReservationBusinessId = (Guid)user.ReservationBusinessId;
            }

            var events = _concertRepository.FindBy(x => x.ReservationBusinessId == request.ReservationBusinessId);

            if (request.EventDate != null && request.EventDate != default(DateTime))
                events = events.Where(x => x.Date == request.EventDate);

            if (!string.IsNullOrEmpty(request.SearchText))
                events = events.Where(x => x.Name.Contains(request.SearchText) || x.Description.Contains(request.SearchText));

            var data = Mapper.Map<List<ConcertEvent>>(events);
            return new GetConcertEventsResponse { Model = data.ToList() };
        }
    }
}
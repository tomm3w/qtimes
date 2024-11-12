using AutoMapper;
using common.api.Queries;
using common.dal;
using SeatQ.core.api.Queries.Reservation.GetConcertReservations;
using SeatQ.core.api.Queries.Reservation.GetReservations;
using SeatQ.core.dal.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SeatQ.core.api.Queries.Reservation.GetConcertMetrics
{
    public class GetMetricsQuery : IQuery<GetConcertMetricsResponse, GetConcertMetricsRequest>
    {
        private readonly IGenericRepository<SeatQEntities, UserInfo> _userInfoRepository;
        private readonly IGenericRepository<SeatQEntities, dal.Models.ConcertReservation> _reservationRepository;

        public GetMetricsQuery(IGenericRepository<SeatQEntities, UserInfo> userInfoRepository,
            IGenericRepository<SeatQEntities, dal.Models.ConcertReservation> reservationRepository)
        {
            _userInfoRepository = userInfoRepository;
            _reservationRepository = reservationRepository;
        }

        public GetConcertMetricsResponse Handle(GetConcertMetricsRequest request)
        {
            //var user = _userInfoRepository.FindBy(x => x.UserId == request.UserId).FirstOrDefault();
            //if (user.ReservationBusinessId == null)
            //    throw new Exception("Reservation business not registered.");

            if (request.Date == null) request.Date = DateTime.UtcNow.Date;
            var query = _reservationRepository.FindBy(x => x.ConcertId == request.ConcertId && x.ReservationDate == request.Date);

            var mapData = Mapper.Map<List<ConcertReservationData>>(query);

            var result = new GetConcertMetricsResponse
            {
                TotalGuests = mapData.Sum(x => x.Size ?? 0),
                TotalEvents = mapData.Count == 0 ? 0 : mapData.Select(x => x.ConcertId).Distinct().Count(),
                TotalReservations = mapData.Count == 0 ? 0 : mapData.Count()
            };

            return result;
        }
    }
}
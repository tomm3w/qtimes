using common.api.Queries;
using common.dal;
using SeatQ.core.dal.Models;
using System;
using System.Linq;

namespace SeatQ.core.api.Queries.Reservation.GetConcertSummary
{
    public class GetConcertSummaryQuery : IQuery<GetConcertSummaryResponse, GetConcertSummaryRequest>
    {
        private readonly IGenericRepository<SeatQEntities, UserInfo> _userInfoRepository;
        private readonly IGenericRepository<SeatQEntities, dal.Models.Concert> _reservationRepository;

        public GetConcertSummaryQuery(IGenericRepository<SeatQEntities, UserInfo> userInfoRepository,
            IGenericRepository<SeatQEntities, dal.Models.Concert> reservationRepository)
        {
            _userInfoRepository = userInfoRepository;
            _reservationRepository = reservationRepository;
        }

        public GetConcertSummaryResponse Handle(GetConcertSummaryRequest request)
        {
            var user = _userInfoRepository.FindBy(x => x.UserId == request.UserId).FirstOrDefault();
            if (user.ReservationBusinessId == null)
                throw new Exception("Reservation business not registered.");

            var query = _reservationRepository.FindBy(x => x.ReservationBusinessId == user.ReservationBusinessId && x.Id == request.ConcertId).FirstOrDefault();

            if (query != null)
            {
                var result = new GetConcertSummaryResponse
                {
                    BusinessName = query.Name,
                    TotalSpots = (int)(query.TotalCapacity ?? 0),
                    Available = (int)(query.TotalCapacity ?? 0) - (int)query.ConcertReservations.Sum(x => (x.Size ?? 0)),
                    Waiting = 0,
                    InPlace = 0
                };
                return result;
            }
            else
                return new GetConcertSummaryResponse();

        }
    }
}
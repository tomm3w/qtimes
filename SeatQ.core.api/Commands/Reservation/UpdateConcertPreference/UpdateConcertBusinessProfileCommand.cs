using common.api.Commands;
using common.dal;
using SeatQ.core.api.Exceptions;
using SeatQ.core.dal.Models;
using System;
using System.Linq;

namespace SeatQ.core.api.Commands.Reservation.UpdateConcertPreference
{
    public class UpdateConcertPreferenceCommand : ICommand<UpdateConcertPreferenceRequest>
    {
        private readonly IGenericRepository<SeatQEntities, UserInfo> _userInfoRepository;
        private readonly IGenericRepository<SeatQEntities, ReservationBusiness> _reservationRepository;
        private readonly IGenericRepository<SeatQEntities, Concert> _concertRepository;

        public UpdateConcertPreferenceCommand(IGenericRepository<SeatQEntities, UserInfo> userInfoRepository,
            IGenericRepository<SeatQEntities, ReservationBusiness> reservationRepository,
            IGenericRepository<SeatQEntities, Concert> concertRepository)
        {
            _userInfoRepository = userInfoRepository;
            _reservationRepository = reservationRepository;
            _concertRepository = concertRepository;
        }

        public void Handle(UpdateConcertPreferenceRequest request)
        {
            var user = _userInfoRepository.FindBy(x => x.UserId == request.UserId).FirstOrDefault();
            if (user.ReservationBusinessId == null)
                throw new Exception("Reservation business not registered.");

            request.ReservationBusinessId = (Guid)user.ReservationBusinessId;

            var concert = _concertRepository.FindBy(x => x.Id == request.ConcertId && x.ReservationBusinessId == request.ReservationBusinessId).FirstOrDefault();

            if (concert == null)
                throw new NotFoundException();

            concert.OpenDayFrom = request.OpenDayFrom;
            concert.OpenDayTo = request.OpenDayTo;
            concert.OpenHourFrom = request.OpenHourFrom;
            concert.OpenHourTo = request.OpenHourTo;
            concert.EnableTotalCapacity = request.EnableTotalCapacity;
            concert.TotalCapacity = request.TotalCapacity;
            concert.EnableGroupSize = request.EnableGroupSize;
            concert.MinGroupSize = request.MinGroupSize;
            concert.MaxGroupSize = request.MaxGroupSize;
            concert.EnableTimeExpiration = request.EnableTimeExpiration;
            concert.TimeExpiration = request.TimeExpiration;
            concert.IsDLScanEnabled = request.IsDLScanEnabled;
            concert.IsChargeEnabled = request.IsChargeEnabled;
            concert.IsSeatmapEnabled = request.IsSeatmapEnabled;

            _concertRepository.Save();
        }
    }
}
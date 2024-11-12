using common.api.Commands;
using common.dal;
using SeatQ.core.api.Exceptions;
using SeatQ.core.dal.Models;
using System;
using System.Linq;

namespace SeatQ.core.api.Commands.Reservation.AddConcertEvent
{
    public class AddConcertEventCommand : ICommand<AddConcertEventRequest>
    {
        private readonly IGenericRepository<SeatQEntities, Concert> _concertRepository;
        private readonly IGenericRepository<SeatQEntities, UserInfo> _userInfoRepository;
        public AddConcertEventCommand(IGenericRepository<SeatQEntities, Concert> concertRepository,
            IGenericRepository<SeatQEntities, UserInfo> userInfoRepository)
        {
            _concertRepository = concertRepository;
            _userInfoRepository = userInfoRepository;
        }
        public void Handle(AddConcertEventRequest request)
        {
            if (request.ReservationBusinessId == null || request.ReservationBusinessId == default(Guid))
            {
                var user = _userInfoRepository.FindBy(x => x.UserId == request.UserId).FirstOrDefault();
                if (user.ReservationBusinessId == null)
                    throw new NotFoundException();

                request.ReservationBusinessId = (Guid)user.ReservationBusinessId;
            }

            var concert = new Concert
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Date = request.Date,
                TimeFrom = request.TimeFrom,
                TimeTo = request.TimeTo,
                ReservationBusinessId = request.ReservationBusinessId
            };

            _concertRepository.Add(concert);
            _concertRepository.Save();
        }
    }
}
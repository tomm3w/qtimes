using common.api.Commands;
using common.dal;
using SeatQ.core.api.Exceptions;
using SeatQ.core.dal.Models;
using System.Linq;
using WebGrease.Css.Extensions;

namespace SeatQ.core.api.Commands.Reservation.AddConcertSeating
{
    public class AddConcertSeatingCommand : ICommand<AddConcertSeatingRequest>
    {
        private readonly IGenericRepository<SeatQEntities, Concert> _concertRepository;
        private readonly IGenericRepository<SeatQEntities, ConcertSeating> _concertSeatingRepository;
        private readonly IGenericRepository<SeatQEntities, UserInfo> _userInfoRepository;
        public AddConcertSeatingCommand(IGenericRepository<SeatQEntities, Concert> concertRepository,
            IGenericRepository<SeatQEntities, ConcertSeating> concertSeatingRepository,
            IGenericRepository<SeatQEntities, UserInfo> userInfoRepository)
        {
            _concertRepository = concertRepository;
            _userInfoRepository = userInfoRepository;
            _concertSeatingRepository = concertSeatingRepository;
        }

        public void Handle(AddConcertSeatingRequest request)
        {
            var concert = _concertRepository.FindBy(x => x.Id == request.ConcertId).FirstOrDefault();

            if (concert == null)
                throw new NotFoundException();

            concert.SeatMapPath = request.SeatMapPath;
            concert.SeatType = request.SeatType;
            concert.AllowCheckInTime = request.AllowCheckInTime;

            var seatings = _concertSeatingRepository.FindBy(x => x.ConcertId == request.ConcertId);
            if (seatings.Any())
            {
                seatings.ForEach(item =>
                    _concertSeatingRepository.Delete(item)
                );
                _concertSeatingRepository.Save();
            }

            if (request.ConcertSeatings != null && request.ConcertSeatings.Any())
            {

                foreach (var item in request.ConcertSeatings)
                {
                    var seat = new ConcertSeating()
                    {
                        SeatsPerSpot = item.SeatsPerSpot,
                        Name = item.Name,
                        Spots = item.Spots,
                        CheckInTime = item.CheckInTime
                    };

                    concert.ConcertSeatings.Add(seat);

                }

            }

            _concertRepository.Save();
        }
    }
}
using iVeew.common.api.Queries;
using IVeew.common.dal;
using QTimes.api.Infrastructure;
using QTimes.core.dal.Models;
using System.Collections.Generic;
using System.Linq;

namespace QTimes.api.Queries.Reservation.GetConcertEventSeatings
{
    public class GetConcertEventSeatingsQuery : IQuery<GetConcertEventSeatingsResponse, GetConcertEventSeatingsRequest>
    {
        private readonly IGenericRepository<QTimesContext, ConcertEventSeating> _seatingRepository;
        private readonly IBlobService _blobService;
        public GetConcertEventSeatingsQuery(IGenericRepository<QTimesContext, ConcertEventSeating> seatingRepository, IBlobService blobService)
        {
            _seatingRepository = seatingRepository;
            _blobService = blobService;
        }
        public GetConcertEventSeatingsResponse Handle(GetConcertEventSeatingsRequest request)
        {
            var seating = _seatingRepository.FindBy(x => x.ConcertEventId == request.ConcertEventId);

            var seatingMap = string.Empty;
            if (seating.Any())
            {
                var seat = seating.FirstOrDefault();
                seatingMap = seat.ConcertEvent.SeatMapPath ?? "";

                var seatings = new List<Seating>();
                seating.ToList().ForEach(x =>
                {
                    seatings.Add(new Seating { Name = x.Name, Spots = x.Spots, SeatsPerSpot = x.SeatsPerSpot, CheckInTime = x.CheckInTime });
                });

                return new GetConcertEventSeatingsResponse
                {
                    Seatings = seatings,
                    SeatMapFullPath = _blobService.GetFileUrl(seatingMap),
                    EnableTimeExpiration = seat.ConcertEvent.EnableTimeExpiration ?? false,
                    MinGroupSize = (seat.ConcertEvent.EnableGroupSize ?? false) ? seat.ConcertEvent.MinGroupSize ?? 0 : 0,
                    MaxGroupSize = (seat.ConcertEvent.EnableGroupSize ?? false) ? seat.ConcertEvent.MaxGroupSize ?? 0 : 0,
                    IsDlscanEnabled = seat.ConcertEvent.IsDlscanEnabled ?? false
                };
            }

            return new GetConcertEventSeatingsResponse();
        }
    }
}
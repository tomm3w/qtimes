using common.api.Queries;
using common.dal;
using SeatQ.core.dal.Models;
using System.Collections.Generic;
using WebGrease.Css.Extensions;
using System.Linq;
using SeatQ.core.api.Exceptions;

namespace SeatQ.core.api.Queries.Reservation.GetConcertSeatings
{
    public class GetConcertSeatingsQuery : IQuery<GetConcertSeatingsResponse, GetConcertSeatingsRequest>
    {
        private readonly IGenericRepository<SeatQEntities, ConcertSeating> _seatingRepository;
        public GetConcertSeatingsQuery(IGenericRepository<SeatQEntities, ConcertSeating> seatingRepository)
        {
            _seatingRepository = seatingRepository;
        }
        public GetConcertSeatingsResponse Handle(GetConcertSeatingsRequest request)
        {
            var seating = _seatingRepository.FindBy(x => x.ConcertId == request.ConcertId);

            var seatingMap = string.Empty;
            if (seating.Any())
            {
                seatingMap = seating.FirstOrDefault().Concert.SeatMapPath ?? "";

                var seatings = new List<Seating>();
                seating.ForEach(x =>
                {
                    seatings.Add(new Seating { Name = x.Name, Spots = x.Spots, SeatsPerSpot = x.SeatsPerSpot, CheckInTime = x.CheckInTime });
                });

                return new GetConcertSeatingsResponse
                {
                    Seatings = seatings,
                    SeatMapFullPath = SeatQ.core.api.Helpers.ImageHelper.ContentAbsUrl("/Content/Uploads/Reservation/" + seatingMap),
                    EnableTimeExpiration = seating.FirstOrDefault().Concert.EnableTimeExpiration ?? false,
                    MinGroupSize = seating.FirstOrDefault().Concert.MinGroupSize ?? 0,
                    MaxGroupSize = seating.FirstOrDefault().Concert.MaxGroupSize ?? 0,
                };
            }
            else
                throw new NotFoundException("Seatings not found");
        }
    }
}
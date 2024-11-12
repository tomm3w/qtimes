using AutoMapper;
using common.api.Queries;
using common.dal;
using SeatQ.core.api.Queries.Reservation.GetReservations;
using SeatQ.core.dal.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SeatQ.core.api.Queries.Reservation.GetSummary
{
    public class GetSummaryQuery : IQuery<GetSummaryResponse, GetSummaryRequest>
    {
        private readonly IGenericRepository<SeatQEntities, dal.Models.Reservation> _reservationRepository;
        private readonly IGenericRepository<SeatQEntities, dal.Models.BusinessDetail> _businessRepository;

        public GetSummaryQuery(
            IGenericRepository<SeatQEntities, dal.Models.Reservation> reservationRepository,
            IGenericRepository<SeatQEntities, dal.Models.BusinessDetail> businessRepository
            )
        {
            _reservationRepository = reservationRepository;
            _businessRepository = businessRepository;
        }

        public GetSummaryResponse Handle(GetSummaryRequest request)
        {
            var business = _businessRepository.FindBy(x => x.Id == request.BusinessDetailId).FirstOrDefault();
            if (business == null)
                throw new Exception("Reservation business not registered.");

            if (request.Date == null) request.Date = DateTime.UtcNow.Date;
            var query = _reservationRepository.FindBy(x => x.BusinessDetailId == request.BusinessDetailId && x.Date == request.Date);

            var mapData = Mapper.Map<List<ReservationData>>(query);

            var limitSize = 0;
            if ((business.LimitSizePerGroup ?? 0) > (business.LimitSizePerHour ?? 0))
                limitSize = business.LimitSizePerHour ?? 0;
            else
                limitSize = business.LimitSizePerGroup ?? 0;

            var result = new GetSummaryResponse
            {
                BusinessName = business.BusinessName,
                LimitSize = limitSize,
                TotalSpots = mapData.Count(),
                Available = mapData.Where(x => x.IsCancelled == true).Count(),
                Waiting = mapData.Where(x => x.TimeIn == null && x.IsCancelled != true).Count(),
                InPlace = mapData.Where(x => x.TimeIn != null && x.IsCancelled != true).Count()
            };

            return result;
        }
    }
}
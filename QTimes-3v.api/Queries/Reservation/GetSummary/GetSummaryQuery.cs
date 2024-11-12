using AutoMapper;
using iVeew.common.api.Queries;
using IVeew.common.dal;
using QTimes.api.Queries.Reservation.GetReservations;
using QTimes.core.dal.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QTimes.api.Queries.Reservation.GetSummary
{
    public class GetSummaryQuery : IQuery<GetSummaryResponse, GetSummaryRequest>
    {
        private readonly IGenericRepository<QTimesContext, QTimes.core.dal.Models.Reservation> _reservationRepository;
        private readonly IGenericRepository<QTimesContext, QTimes.core.dal.Models.BusinessDetail> _businessRepository;

        public GetSummaryQuery(
            IGenericRepository<QTimesContext, QTimes.core.dal.Models.Reservation> reservationRepository,
            IGenericRepository<QTimesContext, QTimes.core.dal.Models.BusinessDetail> businessRepository
            )
        {
            _reservationRepository = reservationRepository;
            _businessRepository = businessRepository;
        }

        public GetSummaryResponse Handle(GetSummaryRequest request)
        {
            if (request.BusinessDetailId == null || request.BusinessDetailId == default(Guid))
                return new GetSummaryResponse();

            var business = _businessRepository.FindBy(x => x.Id == request.BusinessDetailId).FirstOrDefault();
            if (business == null)
                throw new Exception("Reservation business not registered.");

            if (request.Date == null) request.Date = DateTime.UtcNow.Date;
            var query = _reservationRepository.FindBy(x => x.BusinessDetailId == request.BusinessDetailId && x.Date == request.Date
            && request.CurrentTime >= x.TimeFrom && request.CurrentTime <= x.TimeTo && x.IsCancelled != true);

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
                TotalSpots = business.LimitSizePerHour ?? 0,
                Available = (business.LimitSizePerHour ?? 0) - (mapData.Sum(x => x.Size) ?? 0),
                Waiting = mapData.Where(x => x.TimeIn == null).Sum(x => x.Size) ?? 0,
                InPlace = mapData.Where(x => x.TimeIn != null).Sum(x => x.Size) ?? 0,
                EnableGroupSize = business.IsLimitSizePerGroup ?? false,
                MaxGroupSize = business.LimitSizePerGroup ?? 0
            };

            return result;
        }
    }
}
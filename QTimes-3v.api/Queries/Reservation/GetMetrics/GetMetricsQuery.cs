using AutoMapper;
using iVeew.common.api.Queries;
using IVeew.common.dal;
using QTimes.api.Queries.Reservation.GetReservations;
using QTimes.core.dal.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QTimes.api.Queries.Reservation.GetMetrics
{
    public class GetMetricsQuery : IQuery<GetMetricsResponse, GetMetricsRequest>
    {
        private readonly IGenericRepository<QTimesContext, UserInfo> _userInfoRepository;
        private readonly IGenericRepository<QTimesContext, QTimes.core.dal.Models.Reservation> _reservationRepository;
        private readonly IGenericRepository<QTimesContext, BusinessDetail> _businessRepository;

        public GetMetricsQuery(IGenericRepository<QTimesContext, UserInfo> userInfoRepository,
            IGenericRepository<QTimesContext, QTimes.core.dal.Models.Reservation> reservationRepository,
            IGenericRepository<QTimesContext, BusinessDetail> businessRepository)
        {
            _userInfoRepository = userInfoRepository;
            _reservationRepository = reservationRepository;
            _businessRepository = businessRepository;
        }

        public GetMetricsResponse Handle(GetMetricsRequest request)
        {
            var business = _businessRepository.FindBy(x => x.Id == request.BusinessDetailId).FirstOrDefault();
            if (business == null)
                throw new Exception("Reservation business not registered.");

            var query = _reservationRepository.FindBy(x => x.BusinessDetailId == request.BusinessDetailId && x.Date >= request.StartDate && x.Date <= request.EndDate);

            var mapData = Mapper.Map<List<ReservationData>>(query);

            var result = new GetMetricsResponse
            {
                AvgGroupSize = mapData.Count == 0 ? 0 : mapData.Average(x => x.Size ?? 0),
                Cancelled = mapData.Where(x => x.IsCancelled == true).Count(),
                NewGuest = mapData.Count(),
                NoOfPlace = mapData.Where(x => x.TimeIn != null && x.IsCancelled != true).Count(),
                AvgWaitTime = 0//mapData.Count == 0 ? 0 : mapData.Average(x => x.WaitTime)//not needed
            };

            return result;
        }
    }
}
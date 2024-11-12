﻿using AutoMapper;
using common.api.Queries;
using common.dal;
using QTimes.api.Queries.Reservation.GetMetrics;
using SeatQ.core.api.Queries.Reservation.GetReservations;
using SeatQ.core.dal.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SeatQ.core.api.Queries.Reservation.GetMetrics
{
    public class GetMetricsQuery : IQuery<GetMetricsResponse, GetMetricsRequest>
    {
        private readonly IGenericRepository<SeatQEntities, UserInfo> _userInfoRepository;
        private readonly IGenericRepository<SeatQEntities, dal.Models.Reservation> _reservationRepository;
        private readonly IGenericRepository<SeatQEntities, BusinessDetail> _businessRepository;

        public GetMetricsQuery(IGenericRepository<SeatQEntities, UserInfo> userInfoRepository,
            IGenericRepository<SeatQEntities, dal.Models.Reservation> reservationRepository,
            IGenericRepository<SeatQEntities, BusinessDetail> businessRepository)
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

            if (request.Date == null) request.Date = DateTime.UtcNow.Date;
            var query = _reservationRepository.FindBy(x => x.BusinessDetailId == request.BusinessDetailId && x.Date == request.Date);

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
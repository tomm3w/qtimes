﻿using AutoMapper;
using iVeew.common.api.Queries;
using IVeew.common.dal;
using QTimes.core.dal.Models;
using System;
using System.Collections.Generic;
using System.Linq;


namespace QTimes.api.Queries.Reservation.GetReservations
{
    public class GetReservationsQuery : IQuery<GetReservationsResponse, GetReservationsRequest>
    {
        private readonly IGenericRepository<QTimesContext, QTimes.core.dal.Models.Reservation> _repository;
        private readonly IGenericRepository<QTimesContext, BusinessDetail> _businessRepository;

        public GetReservationsQuery(IGenericRepository<QTimesContext, QTimes.core.dal.Models.Reservation> repository,
            IGenericRepository<QTimesContext, BusinessDetail> businessRepository)
        {
            _repository = repository;
            _businessRepository = businessRepository;
        }

        public GetReservationsResponse Handle(GetReservationsRequest request)
        {
            var business = _businessRepository.FindBy(x => x.Id == request.BusinessDetailId).FirstOrDefault();
            if (business == null)
                throw new Exception("Reservation business not registered.");

            var reservations = _repository.FindBy(x => x.BusinessDetailId == request.BusinessDetailId);

            if (request.ReservationDate != null && request.ReservationDate != default(DateTime))
                reservations = reservations.Where(x => x.Date == request.ReservationDate);
            if (!string.IsNullOrEmpty(request.SearchText))
                reservations = reservations.Where(x => x.ReservationGuest.Name.Contains(request.SearchText) || x.ReservationGuest.MobileNumber.Contains(request.SearchText));

            var mapData = Mapper.Map<List<ReservationData>>(reservations);
            int numOfRecords = reservations.Count();
            var paging = new PagingModel
            {
                Page = request.Page,
                PageSize = request.PageSize,
                TotalData = numOfRecords,
                TotalPages = (int)Math.Ceiling((decimal)numOfRecords / (decimal)request.PageSize)
            };

            return new GetReservationsResponse(mapData, paging);

        }

    }
}
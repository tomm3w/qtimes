using AutoMapper;
using common.api.Queries;
using common.dal;
using SeatQ.core.api.Queries.Reservation.GetTimeSlots;
using SeatQ.core.dal.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SeatQ.core.api.Queries.Reservation.GetReservationBusinessById
{
    public class GetReservationBusinessByIdQuery : IQuery<GetReservationBusinessByIdResponse, GetReservationBusinessByIdRequest>
    {
        private readonly IGenericRepository<SeatQEntities, BusinessDetail> _reservationRepository;

        public GetReservationBusinessByIdQuery(IGenericRepository<SeatQEntities, BusinessDetail> reservationRepository)
        {
            _reservationRepository = reservationRepository;
        }

        public GetReservationBusinessByIdResponse Handle(GetReservationBusinessByIdRequest request)
        {

            var business = _reservationRepository.FindBy(x => x.Id == request.Id).FirstOrDefault();

            if (business == null)
                throw new Exception("Reservation business not registered.");

            var result = Mapper.Map<GetReservationBusinessByIdResponse>(business);
            result.TimeSlots = GetAvilableTimeSlots(request);
            return result;
        }

        private List<TimeSlot> GetAvilableTimeSlots(GetReservationBusinessByIdRequest request)
        {
            var timeslots = new List<TimeSlot>();
            using (var context = new SeatQEntities())
            {
                var result = context.GetTimeSlots(request.Id, request.Date);
                timeslots.AddRange(result.Select(x => new TimeSlot
                {
                    OpenHourFrom = x.OpenHourFrom,
                    OpenHourTo = x.OpenHourTo,
                    SpotLeft = x.SpotLeft
                }));
            }

            return timeslots;
        }
    }
}
using common.api.Queries;
using common.dal;
using SeatQ.core.dal.Models;
using System;
using System.Linq;

namespace SeatQ.core.api.Queries.Reservation.GetTimeSlots
{
    public class GetTimeSlotsQuery : IQuery<GetTimeSlotsResponse, GetTimeSlotsRequest>
    {
        private readonly IGenericRepository<SeatQEntities, UserInfo> _userInfoRepository;
        private readonly IGenericRepository<SeatQEntities, dal.Models.Reservation> _reservationRepository;


        public GetTimeSlotsQuery(IGenericRepository<SeatQEntities, UserInfo> userInfoRepository,
            IGenericRepository<SeatQEntities, dal.Models.Reservation> reservationRepository)
        {
            _userInfoRepository = userInfoRepository;
            _reservationRepository = reservationRepository;
        }

        public GetTimeSlotsResponse Handle(GetTimeSlotsRequest request)
        {
            var result = GetAvilableTimeSlots(request);
            return result;
        }

        private GetTimeSlotsResponse GetAvilableTimeSlots(GetTimeSlotsRequest request)
        {
            var timeslots = new GetTimeSlotsResponse();
            using (var context = new SeatQEntities())
            {
                var result = context.GetTimeSlots(request.BusinessId, request.Date);
                timeslots.TimeSlots.AddRange(result.Select(x => new TimeSlot
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
using common.api.Queries;
using System;
using System.Collections.Generic;

namespace SeatQ.core.api.Queries.Reservation.GetTimeSlots
{
    public class GetTimeSlotsResponse : IQueryResponse
    {
        public GetTimeSlotsResponse()
        {
            TimeSlots = new List<TimeSlot>();
        }
        public List<TimeSlot> TimeSlots { get; set; }
    }

    public class TimeSlot
    {
        public TimeSpan? OpenHourFrom { get; set; }
        public TimeSpan? OpenHourTo { get; set; }
        public int? SpotLeft { get; set; }
    }
}
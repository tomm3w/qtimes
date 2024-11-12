using iVeew.common.api.Queries;
using QTimes.core.dal.Models;
using System;
using System.Collections.Generic;

namespace QTimes.api.Queries.Reservation.GetTimeSlots
{
    public class GetTimeSlotsResponse : IQueryResponse
    {
        public GetTimeSlotsResponse()
        {
            TimeSlots = new List<TimeSlot>();
        }
        public List<TimeSlot> TimeSlots { get; set; }
    }
}
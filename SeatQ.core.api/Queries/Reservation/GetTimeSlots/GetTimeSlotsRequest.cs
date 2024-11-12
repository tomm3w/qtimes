using common.api.Queries;
using System;

namespace SeatQ.core.api.Queries.Reservation.GetTimeSlots
{
    public class GetTimeSlotsRequest : IQueryRequest
    {
        public Guid UserId { get; set; }
        public DateTime? Date { get; set; }
        public Guid? BusinessId { get; set; }
    }
}
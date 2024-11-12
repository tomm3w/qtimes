using iVeew.common.api.Commands;
using QTimes.api.Queries.Reservation.GetConcertById;
using System;
using System.Collections.Generic;

namespace QTimes.api.Commands.Reservation.AddConcertEventSeating
{
    public class AddConcertEventSeatingRequest : ICommandRequest
    {
        public Guid UserId { get; set; }
        public string SeatMapPath { get; set; }
        public string SeatType { get; set; }
        public Nullable<bool> AllowCheckInTime { get; set; }
        public List<ConcertSpot> ConcertSeatings { get; set; }
        public Guid EventId { get; set; }
    }
}
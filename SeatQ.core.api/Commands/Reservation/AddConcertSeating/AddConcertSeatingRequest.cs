using common.api.Commands;
using SeatQ.core.api.Queries.Reservation.GetConcertById;
using System;
using System.Collections.Generic;

namespace SeatQ.core.api.Commands.Reservation.AddConcertSeating
{
    public class AddConcertSeatingRequest : ICommandRequest
    {
        public Guid UserId { get; set; }
        public Guid ConcertId { get; set; }
        public string SeatMapPath { get; set; }
        public string SeatType { get; set; }
        public Nullable<bool> AllowCheckInTime { get; set; }
        public List<ConcertSpot> ConcertSeatings { get; set; }
    }
}
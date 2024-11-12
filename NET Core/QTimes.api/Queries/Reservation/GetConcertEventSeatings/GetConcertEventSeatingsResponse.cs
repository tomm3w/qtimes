using iVeew.common.api.Queries;
using System;
using System.Collections.Generic;

namespace QTimes.api.Queries.Reservation.GetConcertEventSeatings
{
    public class GetConcertEventSeatingsResponse : IQueryResponse
    {
        public int MinGroupSize { get; set; }
        public int MaxGroupSize { get; set; }
        public bool EnableTimeExpiration { get; set; }
        public string SeatMapFullPath { get; set; }
        public List<Seating> Seatings { get; set; }
        public bool IsDlscanEnabled { get; set; }
    }
    public class Seating
    {
        public string Name { get; set; }
        public Nullable<short> Spots { get; set; }
        public Nullable<short> SeatsPerSpot { get; set; }
        public Nullable<System.TimeSpan> CheckInTime { get; set; }
    }
}
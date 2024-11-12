using common.api.Queries;
using SeatQ.core.api.Queries.Reservation.GetTimeSlots;
using System.Collections.Generic;

namespace SeatQ.core.api.Queries.Reservation.GetReservationBusinessById
{
    public class GetReservationBusinessByIdResponse : IQueryResponse
    {
        public GetReservationBusinessByIdResponse()
        {
            TimeSlots = new List<TimeSlot>();
        }

        public int? LimitSizePerHour { get; set; }
        public int? LimitSizePerGroup { get; set; }
        public string BusinessName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string LogoPath { get; set; }
        public string LogoFullPath
        {
            get
            {
                return SeatQ.core.api.Helpers.ImageHelper.ContentAbsUrl("/Content/Uploads/Reservation/" + LogoPath ?? "");
            }
        }
        public string FullAddress
        {
            get
            {
                return $"{Address}, {City}, {State} {Zip}";
            }
        }
        public int LimitSize
        {
            get
            {
                var limitSize = 0;
                if ((LimitSizePerGroup ?? 0) > (LimitSizePerHour ?? 0))
                    limitSize = LimitSizePerHour ?? 0;
                else
                    limitSize = LimitSizePerGroup ?? 0;

                return limitSize;
            }
        }
        public List<TimeSlot> TimeSlots { get; set; }

    }
}
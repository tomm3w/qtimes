using common.api.Queries;
using Core.Extensions;
using SeatQ.core.dal.Models;
using System;
using System.Collections.Generic;

namespace SeatQ.core.api.Queries.Reservation.GetReservations
{
    public class GetReservationsResponse : IQueryResponse
    {
        public List<ReservationData> Model { get; set; }
        public PagingModel PagingModel { get; set; }
        public GetReservationsResponse(List<ReservationData> model, PagingModel pagingModel)
        {
            Model = model;
            PagingModel = pagingModel;
        }
    }

    public class ReservationData
    {
        public int Id { get; set; }
        public Nullable<System.Guid> ReservationGuestId { get; set; }
        public Nullable<System.Guid> ReservationBusinessId { get; set; }
        public Nullable<int> GuestTypeId { get; set; }
        public Nullable<short> Size { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public Nullable<System.TimeSpan> TimeFrom { get; set; }
        public Nullable<System.TimeSpan> TimeTo { get; set; }
        public Nullable<System.DateTime> TimeIn { get; set; }
        public Nullable<System.DateTime> TimeOut { get; set; }
        public Nullable<System.DateTime> UtcDateTimeFrom { get; set; }
        public Nullable<System.DateTime> UtcDateTimeTo { get; set; }
        public string Comments { get; set; }
        public Nullable<bool> IsCancelled { get; set; }
        public string GuestName { get; set; }
        public string GuestMobileNumber { get; set; }
        public string GuestType { get; set; }
        public string UniversalDateTimeIn => TimeIn.ToUniversalTimeString();
        public string UniversalDateTimeOut => TimeOut.ToUniversalTimeString();
        public double ReservedMinutes
        {
            get
            {
                if (TimeFrom != null && TimeTo != null)
                    return TimeTo.Value.TotalMinutes - TimeFrom.Value.TotalMinutes;
                else
                    return 0;
            }
        }
        public bool IsTimeOut
        {
            get
            {
                if (!(IsCancelled ?? false) && TimeIn != null)
                {
                    var dateJoined = TimeIn.Value.AddMinutes(ReservedMinutes);
                    if (dateJoined > DateTime.UtcNow)
                        return true;
                    else
                        return false;
                }
                else
                    return false;

            }
        }
        public double WaitTime
        {
            get
            {
                if (TimeFrom != null && TimeIn != null)
                {
                    var date = Date.Value.Add((TimeSpan)TimeFrom);
                    return date.Subtract((DateTime)TimeIn).TotalMinutes;
                }
                else
                    return 0;
            }
        }
        public string UniversalDateTimeFrom => UtcDateTimeFrom.ToUniversalTimeString();
        public string UniversalDateTimeTo => UtcDateTimeTo.ToUniversalTimeString();
    }
}
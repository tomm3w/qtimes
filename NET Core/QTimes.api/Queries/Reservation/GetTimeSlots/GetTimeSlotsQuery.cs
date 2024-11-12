using iVeew.common.api.Queries;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using QTimes.core.dal.Models;
using System.Linq;

namespace QTimes.api.Queries.Reservation.GetTimeSlots
{
    public class GetTimeSlotsQuery : IQuery<GetTimeSlotsResponse, GetTimeSlotsRequest>
    {
        public GetTimeSlotsQuery()
        {
        }

        public GetTimeSlotsResponse Handle(GetTimeSlotsRequest request)
        {
            var result = GetAvilableTimeSlots(request);
            return result;
        }

        private GetTimeSlotsResponse GetAvilableTimeSlots(GetTimeSlotsRequest request)
        {
            var timeslots = new GetTimeSlotsResponse();
            using (var context = new QTimesContext())
            {
                var param = new SqlParameter[] {
                        new SqlParameter() {
                            ParameterName = "@BusinessId",
                            SqlDbType =  System.Data.SqlDbType.UniqueIdentifier,
                            Direction = System.Data.ParameterDirection.Input,
                            Value = request.BusinessId
                        },
                        new SqlParameter() {
                            ParameterName = "@Date",
                            SqlDbType =  System.Data.SqlDbType.DateTime,
                            Direction = System.Data.ParameterDirection.Input,
                            Value = request.Date
                        }};
                var result = context.TimeSlots.FromSqlRaw($"[dbo].[GetTimeSlots] @BusinessId, @Date", param).ToList();
                ////var result = context.GetTimeSlots(request.BusinessId, request.Date);
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
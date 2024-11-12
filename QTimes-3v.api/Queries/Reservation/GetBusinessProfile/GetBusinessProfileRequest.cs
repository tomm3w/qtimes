using iVeew.common.api.Queries;
using System;

namespace QTimes.api.Queries.Reservation.GetBusinessProfile
{
    public class GetBusinessProfileRequest : IQueryRequest
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; }
    }
}
using iVeew.common.api.Queries;
using QTimes.core.dal.Models;
using System.Collections.Generic;

namespace QTimes.api.Queries.GetGuestType
{
    public class GetGuestTypeResponse : IQueryResponse
    {
        public List<GuestType> GuestType { get; private set; }

        public GetGuestTypeResponse(List<GuestType> guestType)
        {
            GuestType = guestType;
        }
    }
}
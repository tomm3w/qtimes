using common.api.Queries;
using SeatQ.core.dal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SeatQ.core.api.Queries.GetGuestType
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
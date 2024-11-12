using common.api.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SeatQ.core.api.Queries.GetRestaurantBusiness
{
    public class GetRestaurantBusinessRequest : IQueryRequest
    {
        public GetRestaurantBusinessRequest(int businessId, Guid userId)
		{
            BusinessId = businessId;
            UserId = userId;
		}

        public int BusinessId { get; private set; }
        public Guid UserId { get; set; }
    }
}
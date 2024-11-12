using common.api.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SeatQ.core.api.Queries.GetUserRestaurants
{
    public class GetUserRestaurantsRequest : IQueryRequest
    {
        public GetUserRestaurantsRequest(Guid userId)
		{
            UserId = userId;
		}

        public Guid UserId { get; private set; }
    }
}
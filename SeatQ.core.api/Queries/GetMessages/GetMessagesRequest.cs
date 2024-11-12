using common.api.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SeatQ.core.api.Queries.GetMessages
{
    public class GetMessagesRequest : IQueryRequest
    {
        public GetMessagesRequest(int restaurantChainId)
        {
            RestaurantChainId = restaurantChainId;
        }

        public int RestaurantChainId { get; private set; }
    }
}
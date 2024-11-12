using common.api.Queries;

namespace SeatQ.core.api.Queries.GetVisitMessages
{
    public class GetVisitMessagesRequest : IQueryRequest
    {
        public GetVisitMessagesRequest(int restaurantChainId)
        {
            RestaurantChainId = restaurantChainId;
        }

        public int RestaurantChainId { get; private set; }
    }
}
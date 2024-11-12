using common.api.Queries;

namespace SeatQ.core.api.Queries.GetReadyMessages
{
    public class GetReadyMessagesRequest : IQueryRequest
    {
        public GetReadyMessagesRequest(int restaurantChainId)
        {
            RestaurantChainId = restaurantChainId;
        }

        public int RestaurantChainId { get; private set; }
    }
}
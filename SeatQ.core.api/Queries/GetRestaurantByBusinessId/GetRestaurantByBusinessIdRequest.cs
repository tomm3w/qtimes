using common.api.Queries;

namespace SeatQ.core.api.Queries.GetRestaurantByBusinessId
{
    public class GetRestaurantByBusinessIdRequest : IQueryRequest
    {
        public GetRestaurantByBusinessIdRequest(int businessId)
		{
            BusinessId = businessId;
		}

        public int BusinessId { get; private set; }
    }
}
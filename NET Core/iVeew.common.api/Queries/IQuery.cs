namespace iVeew.common.api.Queries
{
	public interface IQuery<out TResponse, in TRequest>
		where TRequest : IQueryRequest
		where TResponse : IQueryResponse
	{
		TResponse Handle(TRequest request);
	}
}
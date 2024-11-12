using common.api.Commands;
using common.api.Queries;
using System.Threading.Tasks;

namespace common.api
{
	public interface IApplicationFacade
	{
		TResponse Query<TResponse, TRequest>(TRequest request)
			where TResponse : IQueryResponse
			where TRequest : IQueryRequest;

		void Command<TRequest>(TRequest request) where TRequest : ICommandRequest;
		Task<TResponse> CommandAsync<TRequest, TResponse>(TRequest request) where TRequest : IAsyncCommandRequest<TResponse>;
	}
}

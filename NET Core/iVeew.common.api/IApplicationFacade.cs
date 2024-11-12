using iVeew.common.api.Commands;
using iVeew.common.api.Queries;
using System.Threading.Tasks;

namespace iVeew.common.api
{
	public interface IApplicationFacade
	{
		TResponse Query<TResponse, TRequest>(TRequest request)
			where TResponse : IQueryResponse
			where TRequest : IQueryRequest;

		void Command<TRequest>(TRequest request) where TRequest : ICommandRequest;
		Task<TResponse> CommandAsync<TRequest, TResponse>(TRequest request) where TRequest : IAsyncCommandRequest;
	}
}

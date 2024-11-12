using iVeew.common.api.Commands;
using iVeew.common.api.Queries;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace iVeew.common.api
{
    public class ApplicationFacade : IApplicationFacade
	{
		private readonly IServiceProvider _factory;

		public ApplicationFacade(IServiceProvider factory)
		{
			_factory = factory;
		}

		public TResponse Query<TResponse, TRequest>(TRequest request)
			where TResponse : IQueryResponse
			where TRequest : IQueryRequest
		{
			var query = _factory.GetRequiredService<IQuery<TResponse, TRequest>>();
			return query.Handle(request);
		}

		public void Command<TRequest>(TRequest request)
			where TRequest : ICommandRequest
		{
			
			var command = _factory.GetRequiredService<ICommand<TRequest>>();
			command.Handle(request);
		}

        public Task<TResponse> CommandAsync<TRequest, TResponse>(TRequest request) where TRequest : IAsyncCommandRequest
        {
			var command = _factory.GetRequiredService<IAsyncCommand<TRequest, TResponse>>();
			return command.Handle(request);
		}
    }
}

using System.Threading.Tasks;

namespace common.api.Commands
{
    public interface IAsyncCommand<in TRequest, TResponse> where TRequest : IAsyncCommandRequest<TResponse>
	{
		Task<TResponse> Handle(TRequest request);
	}
}

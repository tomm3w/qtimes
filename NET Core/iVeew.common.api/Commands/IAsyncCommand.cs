using System.Threading.Tasks;

namespace iVeew.common.api.Commands
{
    public interface IAsyncCommand<TRequest, TResponse> where TRequest : IAsyncCommandRequest
	{
		Task<TResponse> Handle(TRequest request);
	}
}

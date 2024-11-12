namespace iVeew.common.api.Commands
{
	public interface ICommand<in TRequest> where TRequest : ICommandRequest
	{
		void Handle(TRequest request);
	}
}
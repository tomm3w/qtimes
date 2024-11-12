using System.Net;
using common.api.Infrastructure.Exeptions;

namespace SeatQ.core.api.Exceptions
{
	public class UsernameDuplicationException : ApiException
	{
		public UsernameDuplicationException()
			: base("User with same username exists in database.", HttpStatusCode.Conflict)
		{
		}
	}
}
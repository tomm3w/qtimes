using System.Net;
using common.api.Infrastructure.Exeptions;

namespace QTimes.api.Exceptions
{
	public class UsernameDuplicationException : ApiException
	{
		public UsernameDuplicationException()
			: base("User with same username exists in database.", HttpStatusCode.Conflict)
		{
		}
	}
}
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;

namespace SeatQ.core.api.Security
{
	public class AuthorizationManager : ClaimsAuthorizationManager
	{
		public override bool CheckAccess(AuthorizationContext context)
		{
			// inspect sub, action, resource
			Debug.WriteLine(context.Principal.FindFirst("sub").Value);
			Debug.WriteLine(context.Action.First().Value);
			Debug.WriteLine(context.Resource.First().Value);

			return true;
		}
	}
}
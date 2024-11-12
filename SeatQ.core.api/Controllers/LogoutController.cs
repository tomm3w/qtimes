using System.Web.Http;
using System.Web.Http.Description;

namespace SeatQ.core.api.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class LogoutController : ApiController
	{
		public void Post()
		{
			Authentication.Web.Security.Logout();
		}
	}
}
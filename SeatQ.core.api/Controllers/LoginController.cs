
using System.Web.Http;
using System.Web.Http.Cors;
using System.Net.Http;
using System.Net;
using SeatQ.core.api.Models;
using System.Web.Http.Description;

namespace SeatQ.core.api.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class LoginController : ApiController
    {
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [AllowAnonymous]
        public HttpResponseMessage Post([FromBody]LoginModel loginModel)
        {
            if (Authentication.Web.Security.Login(loginModel.Username, loginModel.Password))
            {
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            var message = new HttpResponseMessage(HttpStatusCode.Unauthorized)
            {
                Content = new StringContent("Invalid username or password")
            };
            return message;
        }
    }
}
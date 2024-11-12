using common.api;
using common.api.Infrastructure;
using SeatQ.core.api.Commands.AddBusinessDetail;
using SeatQ.core.api.Infrastructure;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SeatQ.core.api.Controllers
{
    [RoutePrefix("api/business")]
    [Authorize(Roles = "Administrator,Regional Manager")]
    [ValidateModel]
    public class BusinessController : SpecificApiController
    {
        public BusinessController(IApplicationFacade applicationFacade)
           : base(applicationFacade)
        { }

        [Route("")]
        public HttpResponseMessage Post(AddBusinessDetailRequest model)
        {
            model.UserId = UserId;
            Facade.Command(model);
            return CreateHttpResponse(HttpStatusCode.OK);
        }
    }
}

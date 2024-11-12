using iVeew.common.api;
using iVeew.common.api.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QTimes.api.Commands.AddBusinessDetail;
using QTimes.api.Commands.Reservation.DeleteBusinessAcount;
using QTimes.api.Infrastructure;

namespace SeatQ.core.api.Controllers
{
    [Authorize]
    [ValidateModel]
    [ApiController]
    [Route("[controller]")]
    public class BusinessController : SpecificApiController
    {
        public BusinessController(IApplicationFacade applicationFacade)
           : base(applicationFacade)
        { }

        //[Route("/business/post")]
        [HttpPost]
        public IActionResult Post([FromForm] AddBusinessDetailRequest model)
        {
            model.UserId = UserId;
            Facade.Command(model);
            return CreateOkHttpResponse();
        }

        //[Route("/business/delete")]
        [HttpDelete]
        public IActionResult Delete([FromForm] DeleteBusinessAcountRequest model)
        {
            model.UserId = UserId;
            Facade.Command(model);
            return CreateOkHttpResponse();
        }
    }
}

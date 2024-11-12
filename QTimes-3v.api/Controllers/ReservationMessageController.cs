using iVeew.common.api;
using iVeew.common.api.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QTimes.api.Infrastructure;
using QTimes.api.Commands.Reservation.AddReservationMessageRule;
using QTimes.api.Commands.Reservation.UpdateReservationMessageRule;
using QTimes.api.Queries.Reservation.GetReservationMessageRule;
using System;
using QTimes.api.Commands.Reservation.DeleteReservationMessageRule;

namespace QTimes.api.Controllers
{
    [Authorize]
    [ValidateModel]
    [ApiController]
    [Route("[controller]")]
    public class ReservationMessageController : SpecificApiController
    {
        public ReservationMessageController(IApplicationFacade applicationFacade)
           : base(applicationFacade)
        { }



        [Route("{BusinessDetailId}")]
        [HttpGet]
        public IActionResult Get(string BusinessDetailId)
        {
            var response = Facade.Query<GetReservationMessageRuleResponse, GetReservationMessageRuleRequest>(new GetReservationMessageRuleRequest { UserId = UserId, BusinessDetailId = Guid.Parse(BusinessDetailId) });
            return CreateHttpResponse(response);
        }

        [Route("")]
        [HttpPost]
        public IActionResult Post([FromForm]AddReservationMessageRuleRequest model)
        {
            model.UserId = UserId;
            Facade.Command(model);
            return CreateOkHttpResponse();
        }
        [Route("")]
        [HttpDelete]
        public IActionResult Delete([FromForm] DeleteReservationMessageRuleRequest model)
        {
            Facade.Command(model);
            return CreateOkHttpResponse();
        }

        [Route("")]
        [HttpPut]
        public IActionResult Put([FromForm]UpdateReservationMessageRuleRequest model)
        {
            Facade.Command(model);
            return CreateOkHttpResponse();
        }

    }
}

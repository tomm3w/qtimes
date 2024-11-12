using iVeew.common.api;
using iVeew.common.api.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QTimes.api.Commands.Reservation.AddConcertEventMessageRule;
using QTimes.api.Commands.Reservation.DeleteConcertEventMessageRule;
using QTimes.api.Commands.Reservation.UpdateConcertEventMessageRule;
using QTimes.api.Infrastructure;
using QTimes.api.Queries.Reservation.GetConcertEventMessageRule;
using System;

namespace QTimes.api.Controllers
{
    [Authorize]
    [ValidateModel]
    [ApiController]
    [Route("[controller]")]
    public class ConcertEventMessageRuleController : SpecificApiController
    {
        public ConcertEventMessageRuleController(IApplicationFacade applicationFacade)
           : base(applicationFacade)
        { }

        [Route("/concerts/{concertId:Guid}/events/{eventId:Guid}/messagerules")]
        [HttpGet]
        public IActionResult Get(Guid concertId, Guid eventId)
        {
            var response = Facade.Query<GetConcertEventMessageRuleResponse, GetConcertEventMessageRuleRequest>(new GetConcertEventMessageRuleRequest { UserId = UserId, ConcertEventId = eventId });
            return CreateHttpResponse(response);
        }

        [Route("/concerts/{concertId:Guid}/events/{eventId:Guid}/messagerules")]
        [HttpPost]
        public IActionResult Post([FromForm] AddConcertEventMessageRuleRequest request, Guid concertId, Guid eventId)
        {
            request.UserId = UserId;
            request.ConcertEventId = eventId;
            Facade.Command(request);
            return CreateOkHttpResponse();
        }

        [Route("/concerts/{concertId:Guid}/events/{eventId:Guid}/messagerules")]
        [HttpPut]
        public IActionResult Put([FromForm] UpdateConcertEventMessageRuleRequest request)
        {
            Facade.Command(request);
            return CreateOkHttpResponse();
        }

        [Route("/concerts/{concertId:Guid}/events/{eventId:Guid}/messagerules/{id:int}")]
        [HttpDelete]
        public IActionResult Delete(int id, Guid eventId)
        {
            Facade.Command(new DeleteConcertEventMessageRuleRequest { Id = id, ConcertEventId = eventId, UserId = UserId });
            return CreateOkHttpResponse();
        }
    }
}

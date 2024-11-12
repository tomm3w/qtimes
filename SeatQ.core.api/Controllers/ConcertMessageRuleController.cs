using common.api;
using common.api.Infrastructure;
using SeatQ.core.api.Commands.Reservation.AddConcertMessageRule;
using SeatQ.core.api.Commands.Reservation.UpdateReservationMessageRule;
using SeatQ.core.api.Infrastructure;
using SeatQ.core.api.Queries.Reservation.GetConcertMessageRule;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SeatQ.core.api.Controllers
{
    [RoutePrefix("api/concertmessagerule")]
    [Authorize(Roles = "Administrator,Regional Manager")]
    [ValidateModel]
    public class ConcertMessageRuleController : SpecificApiController
    {
        public ConcertMessageRuleController(IApplicationFacade applicationFacade)
           : base(applicationFacade)
        { }

        [Route("{id}")]
        public HttpResponseMessage Get(string id)
        {
            var response = Facade.Query<GetConcertMessageRuleResponse, GetConcertMessageRuleRequest>(new GetConcertMessageRuleRequest { UserId = UserId, ConcertId = Guid.Parse(id) });
            return CreateHttpResponse(response);
        }

        [Route("")]
        public HttpResponseMessage Post(AddConcertMessageRuleRequest model)
        {
            model.UserId = UserId;
            Facade.Command(model);
            return CreateHttpResponse(HttpStatusCode.OK);
        }

        [Route("")]
        public HttpResponseMessage Put(UpdateConcertMessageRuleRequest model)
        {
            Facade.Command(model);
            return CreateHttpResponse(HttpStatusCode.OK);
        }
    }
}

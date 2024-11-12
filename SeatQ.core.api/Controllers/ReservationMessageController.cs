using common.api;
using common.api.Infrastructure;
using SeatQ.core.api.Commands.Reservation.AddReservationMessageRule;
using SeatQ.core.api.Commands.Reservation.UpdateReservationMessageRule;
using SeatQ.core.api.Infrastructure;
using SeatQ.core.api.Queries.Reservation.GetReservationMessageRule;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace SeatQ.core.api.Controllers
{
    [RoutePrefix("api/reservationmessage")]
    [Authorize(Roles = "Administrator,Regional Manager")]
    [EnableCors(origins: "*", headers: "*", methods: "*", SupportsCredentials = true)]
    [ValidateModel]
    public class ReservationMessageController : SpecificApiController
    {
        public ReservationMessageController(IApplicationFacade applicationFacade)
           : base(applicationFacade)
        { }

        [Route("{BusinessDetailId}")]
        public HttpResponseMessage Get(string BusinessDetailId)
        {
            var response = Facade.Query<GetReservationMessageRuleResponse, GetReservationMessageRuleRequest>(new GetReservationMessageRuleRequest { UserId = UserId, BusinessDetailId = Guid.Parse(BusinessDetailId) });
            return CreateHttpResponse(response);
        }

        [Route("")]
        public HttpResponseMessage Post(AddReservationMessageRuleRequest model)
        {
            model.UserId = UserId;
            Facade.Command(model);
            return CreateHttpResponse(HttpStatusCode.OK);
        }

        [Route("")]
        public HttpResponseMessage Put(UpdateReservationMessageRuleRequest model)
        {
            Facade.Command(model);
            return CreateHttpResponse(HttpStatusCode.OK);
        }

    }
}

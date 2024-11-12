using common.api;
using common.api.Infrastructure;
using SeatQ.core.api.Commands.Reservation.AddConcertEvent;
using SeatQ.core.api.Commands.Reservation.UpdateConcertEvent;
using SeatQ.core.api.Infrastructure;
using SeatQ.core.api.Queries.Reservation.GetConcertEvents;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SeatQ.core.api.Controllers
{
    [RoutePrefix("api/concertevent")]
    [Authorize(Roles = "Administrator,Regional Manager")]
    [ValidateModel]
    public class ConcertEventController : SpecificApiController
    {
        public ConcertEventController(IApplicationFacade applicationFacade)
           : base(applicationFacade)
        { }

        [Route("")]
        public HttpResponseMessage Get(DateTime? EventDate = null, string SearchText = null)
        {
            var response = Facade.Query<GetConcertEventsResponse, GetConcertEventsRequest>(new GetConcertEventsRequest { UserId = UserId, SearchText = SearchText, EventDate = EventDate });
            return CreateHttpResponse(response);
        }

        [Route("")]
        public HttpResponseMessage Post(AddConcertEventRequest model)
        {
            model.UserId = UserId;
            Facade.Command(model);
            return CreateHttpResponse(HttpStatusCode.OK);
        }

        [Route("")]
        public HttpResponseMessage Put(UpdateConcertEventRequest model)
        {
            model.UserId = UserId;
            Facade.Command(model);
            return CreateHttpResponse(HttpStatusCode.OK);
        }
    }
}

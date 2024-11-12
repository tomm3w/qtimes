using iVeew.common.api;
using iVeew.common.api.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QTimes.api.Commands.Reservation.AddConcertEvent;
using QTimes.api.Commands.Reservation.AddConcertEventSeating;
using QTimes.api.Commands.Reservation.DeleteConcertEvent;
using QTimes.api.Commands.Reservation.UpdateConcertBusinessProfile;
using QTimes.api.Commands.Reservation.UpdateConcertEvent;
using QTimes.api.Commands.Reservation.UpdateConcertEventPreference;
using QTimes.api.Infrastructure;
using QTimes.api.Models.Constants;
using QTimes.api.Queries.Reservation.GetConcertEventById;
using QTimes.api.Queries.Reservation.GetConcertEvents;
using QTimes.api.Queries.Reservation.GetConcertEventSeatings;
using QTimes.api.Queries.Reservation.GetConcertEventSummary;
using System;

namespace QTimes.api.Controllers
{
    //[Authorize]
    //[ValidateModel]
    [ApiController]
    [Route("[controller]")]
    public class ConcertEventController : SpecificApiController
    {
        public ConcertEventController(IApplicationFacade applicationFacade)
           : base(applicationFacade)
        { }

        [AllowAnonymous]
        [Route("/concerts/{concertId:Guid}/events/{eventId:Guid}")]
        [HttpGet]
        public IActionResult Get(Guid concertId, Guid eventId)
        {
            var response = Facade.Query<GetConcertEventByIdResponse, GetConcertEventByIdRequest>(new GetConcertEventByIdRequest { Id = eventId });
            return CreateHttpResponse(response);
        }


        [Route("/concerts/{concertId:Guid}/events")]
        [HttpGet]
        public IActionResult Get(Guid concertId, DateTime? EventDate = null, string SearchText = null, string FilterBy = null)
        {
            var response = Facade.Query<GetConcertEventsResponse, GetConcertEventsRequest>(new GetConcertEventsRequest { UserId = UserId, SearchText = SearchText, EventDate = EventDate, ConcertId = concertId, FilterBy = FilterBy });
            return CreateHttpResponse(response);
        }

        [Route("/concerts/{concertId:Guid}/events")]
        [HttpPost]
        public IActionResult Post([FromForm] AddConcertEventRequest model)
        {
            model.UserId = UserId;
            Facade.Command(model);
            return CreateOkHttpResponse();
        }

        [Route("/concerts/{concertId:Guid}/events/{eventId:Guid}")]
        [HttpPut]
        public IActionResult Put([FromForm] UpdateConcertEventRequest request, Guid concertId, Guid eventId)
        {
            request.UserId = UserId;
            request.ConcertId = concertId;
            request.Id = eventId;
            Facade.Command(request);
            return CreateOkHttpResponse();
        }

        [Route("/concerts/{concertId:Guid}/events/{eventId:Guid}")]
        [HttpDelete]
        public IActionResult Delete([FromForm] DeleteConcertEventRequest request, Guid concertId, Guid eventId)
        {
            request.UserId = UserId;
            request.ConcertId = concertId;
            request.Id = eventId;
            Facade.Command(request);
            return CreateOkHttpResponse();
        }

        [Route("/concerts/{concertId:Guid}/events/{eventId:Guid}/businessprofile")]
        [HttpPut]
        public IActionResult UpdateEventBusinessProfile([FromForm] UpdateConcertEventBusinessProfileRequest request, Guid concertId, Guid eventId)
        {
            request.UserId = UserId;
            request.ConcertId = concertId;
            request.Id = eventId;
            Facade.Command(request);
            return CreateOkHttpResponse();
        }

        [Route("/concerts/{concertId:Guid}/events/{eventId:Guid}/preference")]
        [HttpPut]
        public IActionResult UpdateEventPreference([FromForm] UpdateConcertEventPreferenceRequest request, Guid concertId, Guid eventId)
        {
            request.UserId = UserId;
            request.ConcertId = concertId;
            request.Id = eventId;
            Facade.Command(request);
            return CreateOkHttpResponse();
        }

        [Route("/concerts/{concertId:Guid}/events/{eventId:Guid}/seating")]
        [HttpPut]
        public IActionResult UpdateEventSeating([FromForm] AddConcertEventSeatingRequest request, Guid concertId, Guid eventId)
        {
            request.UserId = UserId;
            request.EventId = eventId;
            Facade.Command(request);

            return CreateOkHttpResponse();
        }

        [Route("/concerts/{:concertId}/summary")]
        [HttpPost]
        public IActionResult GetConcertEventSummary([FromForm] GetConcertEventSummaryRequest request, [FromQuery] Guid? eventId)
        {
            request.UserId = UserId;
            request.EventId = eventId;
            var response = Facade.Query<GetConcertEventSummaryResponse, GetConcertEventSummaryRequest>(request);
            return CreateHttpResponse(response);
        }

        [AllowAnonymous]
        [Route("/concerts/{concertId:Guid}/events/{eventId:Guid}/seatings")]
        [HttpGet]
        public IActionResult GetConcertSeatings(Guid concertId, Guid eventId)
        {
            var request = new GetConcertEventSeatingsRequest()
            {
                ConcertEventId = eventId
            };
            var response = Facade.Query<GetConcertEventSeatingsResponse, GetConcertEventSeatingsRequest>(request);
            return CreateHttpResponse(response);
        }
    }
}

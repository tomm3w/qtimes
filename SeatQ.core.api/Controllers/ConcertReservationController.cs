using common.api;
using common.api.Infrastructure;
using SeatQ.core.api.Commands.Reservation.AddConcertReservation;
using SeatQ.core.api.Commands.Reservation.AddReservation;
using SeatQ.core.api.Commands.Reservation.LockConcertSeat;
using SeatQ.core.api.Commands.Reservation.MessageConcertReservation;
using SeatQ.core.api.Commands.Reservation.UpdateConcertCheckIn;
using SeatQ.core.api.Commands.Reservation.UpdateConcertReservation;
using SeatQ.core.api.Infrastructure;
using SeatQ.core.api.Models.Enums;
using SeatQ.core.api.Queries.Reservation.GetConcertById;
using SeatQ.core.api.Queries.Reservation.GetConcertMessages;
using SeatQ.core.api.Queries.Reservation.GetConcertMetrics;
using SeatQ.core.api.Queries.Reservation.GetConcertReservations;
using SeatQ.core.api.Queries.Reservation.GetConcertSeatings;
using SeatQ.core.api.Queries.Reservation.GetConcertSummary;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;

namespace SeatQ.core.api.Controllers
{
    [RoutePrefix("api/concertreservation")]
    [Authorize(Roles = "Administrator,Regional Manager")]
    [EnableCors(origins: "*", headers: "*", methods: "*", SupportsCredentials = true)]
    [ValidateModel]
    public class ConcertReservationController : SpecificApiController
    {
        public ConcertReservationController(IApplicationFacade applicationFacade)
           : base(applicationFacade)
        { }

        [Route("")]
        public HttpResponseMessage Get(int Page, int PageSize, string ConcertId, string SearchText = null)
        {
            var model = new GetConcertReservationsRequest
            {
                UserId = UserId,
                Page = Page,
                PageSize = PageSize,
                SearchText = SearchText,
                ConcertId = Guid.Parse(ConcertId)
            };

            var response = Facade.Query<GetConcertReservationsResponse, GetConcertReservationsRequest>(model);
            return CreateHttpResponse(response);
        }

        [Route("")]
        public HttpResponseMessage Post(AddConcertReservationRequest model)
        {
            Facade.Command(model);
            return CreateHttpResponse(HttpStatusCode.OK);
        }

        [Route("")]
        public HttpResponseMessage Put(UpdateConcertReservationRequest model)
        {
            Facade.Command(model);
            return CreateHttpResponse(HttpStatusCode.OK);
        }


        [Route("ConcertMessage")]
        [HttpPost]
        public HttpResponseMessage ConcertMessage(MessageConcertReservationRequest model)
        {
            model.MessageSentBy = UserId;
            Facade.Command(model);
            return CreateHttpResponse(HttpStatusCode.OK);
        }

        [Route("Metrics")]
        [HttpPost]
        public HttpResponseMessage Metrics(GetConcertMetricsRequest model)
        {
            model.UserId = UserId;
            var response = Facade.Query<GetConcertMetricsResponse, GetConcertMetricsRequest>(model);
            return CreateHttpResponse(response);
        }

        [Route("GetConcertMessages")]
        [HttpPost]
        public HttpResponseMessage GetConcertMessages(GetConcertMessagesRequest model)
        {
            model.UserId = UserId;
            var response = Facade.Query<GetConcertMessagesResponse, GetConcertMessagesRequest>(model);
            return CreateHttpResponse(response);
        }

        [AllowAnonymous]
        [Route("AddReservation")]
        public async Task<HttpResponseMessage> AddReservation(AddReservationRequest model)
        {
            await Facade.CommandAsync<AddReservationRequest, string>(model);
            return CreateHttpResponse(HttpStatusCode.OK);
        }

        [Route("GetConcertSummary")]
        [HttpPost]
        public HttpResponseMessage GetConcertSummary(GetConcertSummaryRequest model)
        {
            model.UserId = UserId;
            var response = Facade.Query<GetConcertSummaryResponse, GetConcertSummaryRequest>(model);
            return CreateHttpResponse(response);
        }

        [AllowAnonymous]
        [Route("GetConcertSeatings")]
        [HttpPost]
        public HttpResponseMessage GetConcertSeatings(GetConcertSeatingsRequest model)
        {
            var response = Facade.Query<GetConcertSeatingsResponse, GetConcertSeatingsRequest>(model);
            return CreateHttpResponse(response);
        }

        [AllowAnonymous]
        [Route("concert/{id}")]
        public HttpResponseMessage GetConcert(string id)
        {
            var response = Facade.Query<GetConcertByIdResponse, GetConcertByIdRequest>(new GetConcertByIdRequest { Id = Guid.Parse(id) });
            return CreateHttpResponse(response);
        }

        [AllowAnonymous]
        [Route("addconcertreservation")]
        [HttpPost]
        public HttpResponseMessage AddConcertReservation(AddConcertReservationRequest model)
        {
            model.GuestTypeId = (int)GuestTypeEnum.MobileConnected;
            Facade.Command(model);
            return CreateHttpResponse(HttpStatusCode.OK);
        }

        [AllowAnonymous]
        [Route("checkinguest")]
        public HttpResponseMessage GetCheckInGuest(int Page, int PageSize, string ConcertId, string SearchText = null)
        {
            var model = new GetConcertReservationsRequest
            {
                Page = Page,
                PageSize = PageSize,
                SearchText = SearchText,
                ConcertId = Guid.Parse(ConcertId)
            };

            var response = Facade.Query<GetConcertReservationsResponse, GetConcertReservationsRequest>(model);
            return CreateHttpResponse(response);
        }

        [AllowAnonymous]
        [Route("updatecheckin")]
        [HttpPut]
        public HttpResponseMessage UpdateConcertCheckIn(UpdateConcertCheckInRequest model)
        {
            Facade.Command(model);
            return CreateHttpResponse(HttpStatusCode.OK);
        }

        [AllowAnonymous]
        [Route("validateseatlock")]
        [HttpPost]
        public HttpResponseMessage ValidateSeatLock(LockConcertSeatRequest model)
        {
            Facade.Command(model);
            return CreateHttpResponse(HttpStatusCode.OK);
        }

    }
}
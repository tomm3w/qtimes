using common.api;
using common.api.Infrastructure;
using QTimes.api.Queries.Reservation.GetMetrics;
using SeatQ.core.api.Commands.Reservation.AddReservation;
using SeatQ.core.api.Commands.Reservation.CancelReservation;
using SeatQ.core.api.Commands.Reservation.EditReservation;
using SeatQ.core.api.Commands.Reservation.MessageReservation;
using SeatQ.core.api.Commands.Reservation.TimeInReservation;
using SeatQ.core.api.Commands.Reservation.TimeOutReservation;
using SeatQ.core.api.Infrastructure;
using SeatQ.core.api.Queries.Reservation.GetMessages;
using SeatQ.core.api.Queries.Reservation.GetMetrics;
using SeatQ.core.api.Queries.Reservation.GetReservationBusinessById;
using SeatQ.core.api.Queries.Reservation.GetReservations;
using SeatQ.core.api.Queries.Reservation.GetSummary;
using SeatQ.core.api.Queries.Reservation.GetTimeSlots;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;

namespace SeatQ.core.api.Controllers
{
    [RoutePrefix("api/reservation")]
    [Authorize(Roles = "Administrator,Regional Manager")]
    [EnableCors(origins: "*", headers: "*", methods: "*", SupportsCredentials = true)]
    [ValidateModel]
    public class ReservationController : SpecificApiController
    {
        public ReservationController(IApplicationFacade applicationFacade)
           : base(applicationFacade)
        { }

        [Route("")]
        public HttpResponseMessage Get(int Page, int PageSize, string BusinessId, DateTime? ReservationDate = null, string SearchText = null)
        {
            var model = new GetReservationsRequest
            {
                UserId = UserId,
                Page = Page,
                PageSize = PageSize,
                SearchText = SearchText,
                ReservationDate = ReservationDate,
                BusinessDetailId = Guid.Parse(BusinessId)
            };

            var response = Facade.Query<GetReservationsResponse, GetReservationsRequest>(model);
            return CreateHttpResponse(response);
        }

        [Route("")]
        public async Task<HttpResponseMessage> Post(AddReservationRequest model)
        {
            model.CreatedBy = UserId;
            await Facade.CommandAsync<AddReservationRequest, string>(model);
            return CreateHttpResponse(HttpStatusCode.OK);
        }

        [Route("")]
        public HttpResponseMessage Put(EditReservationRequest model)
        {
            model.UserId = UserId;
            Facade.Command(model);
            return CreateHttpResponse(HttpStatusCode.OK);
        }

        [Route("TimeIn")]
        [HttpPost]
        public HttpResponseMessage TimeIn(TimeInReservationRequest model)
        {
            model.UserId = UserId;
            Facade.Command(model);
            return CreateHttpResponse(HttpStatusCode.OK);
        }

        [Route("TimeOut")]
        [HttpPost]
        public HttpResponseMessage TimeIn(TimeOutReservationRequest model)
        {
            model.UserId = UserId;
            Facade.Command(model);
            return CreateHttpResponse(HttpStatusCode.OK);
        }

        [Route("Cancel")]
        [HttpPost]
        public HttpResponseMessage Cancel(CancelReservationRequest model)
        {
            model.UserId = UserId;
            Facade.Command(model);
            return CreateHttpResponse(HttpStatusCode.OK);
        }

        [Route("Message")]
        [HttpPost]
        public HttpResponseMessage Message(MessageReservationRequest model)
        {
            model.MessageSentBy = UserId;
            Facade.Command(model);
            return CreateHttpResponse(HttpStatusCode.OK);
        }

        [Route("Metrics")]
        [HttpPost]
        public HttpResponseMessage Metrics(GetMetricsRequest model)
        {
            model.UserId = UserId;
            var response = Facade.Query<GetMetricsResponse, GetMetricsRequest>(model);
            return CreateHttpResponse(response);
        }

        [Route("GetMessages")]
        [HttpPost]
        public HttpResponseMessage GetMessages(GetMessagesRequest model)
        {
            model.UserId = UserId;
            var response = Facade.Query<GetMessagesResponse, GetMessagesRequest>(model);
            return CreateHttpResponse(response);
        }

        [Route("GetSummary")]
        [HttpPost]
        public HttpResponseMessage GetSummary(GetSummaryRequest model)
        {
            model.UserId = UserId;
            var response = Facade.Query<GetSummaryResponse, GetSummaryRequest>(model);
            return CreateHttpResponse(response);
        }

        [Route("GetTimeSlots")]
        [HttpPost]
        public HttpResponseMessage GetTimeSlots(GetTimeSlotsRequest model)
        {
            model.UserId = UserId;
            var response = Facade.Query<GetTimeSlotsResponse, GetTimeSlotsRequest>(model);
            return CreateHttpResponse(response);
        }

        [AllowAnonymous]
        [Route("business/{id}")]
        [HttpPost]
        public HttpResponseMessage GetTimeSlots(GetReservationBusinessByIdRequest model)
        {
            var response = Facade.Query<GetReservationBusinessByIdResponse, GetReservationBusinessByIdRequest>(model);
            return CreateHttpResponse(response);
        }

        [AllowAnonymous]
        [Route("AddReservation")]
        public async Task<HttpResponseMessage> AddReservation(AddReservationRequest model)
        {
            string passUrl = await Facade.CommandAsync<AddReservationRequest, string>(model);
            return CreateHttpResponse(passUrl);
        }

    }
}
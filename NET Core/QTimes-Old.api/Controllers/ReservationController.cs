using iVeew.common.api;
using iVeew.common.api.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QTimes.api.Commands.Reservation.AddReservation;
using QTimes.api.Commands.Reservation.CancelReservation;
using QTimes.api.Commands.Reservation.EditReservation;
using QTimes.api.Commands.Reservation.MessageReservation;
using QTimes.api.Commands.Reservation.TimeInReservation;
using QTimes.api.Commands.Reservation.TimeOutReservation;
using QTimes.api.Infrastructure;
using QTimes.api.Queries.Reservation.GetMessages;
using QTimes.api.Queries.Reservation.GetMetrics;
using QTimes.api.Queries.Reservation.GetReservationBusinessById;
using QTimes.api.Queries.Reservation.GetReservations;
using QTimes.api.Queries.Reservation.GetSummary;
using QTimes.api.Queries.Reservation.GetTimeSlots;
using System;
using System.Threading.Tasks;

namespace QTimes.api.Controllers
{
    //[Authorize]
    //[ValidateModel]
    [ApiController]
    [Route("[controller]")]
    public class ReservationController : SpecificApiController
    {
        public ReservationController(IApplicationFacade applicationFacade)
           : base(applicationFacade)
        { }

        [Route("")]
        [HttpGet]
        public IActionResult Get(int Page, int PageSize, string BusinessId, DateTime? ReservationDate = null, string SearchText = null)
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
        [HttpPost]
        public async Task<IActionResult> Post([FromForm]AddReservationRequest model)
        {
            model.CreatedBy = UserId;
            var response = await Facade.CommandAsync<AddReservationRequest
                , AddReservationResponse>(model);
            return CreateHttpResponse(response);
        }

        [Route("")]
        [HttpPut]
        public IActionResult Put([FromForm]EditReservationRequest model)
        {
            model.UserId = UserId;
            Facade.Command(model);
            return CreateOkHttpResponse();
        }

        [Route("TimeIn")]
        [HttpPost]
        public IActionResult TimeIn([FromForm]TimeInReservationRequest model)
        {
            model.UserId = UserId;
            Facade.Command(model);
            return CreateOkHttpResponse();
        }

        [Route("TimeOut")]
        [HttpPost]
        public IActionResult TimeOut([FromForm]TimeOutReservationRequest model)
        {
            model.UserId = UserId;
            Facade.Command(model);
            return CreateOkHttpResponse();
        }

        [Route("Cancel")]
        [HttpPost]
        public IActionResult Cancel([FromForm]CancelReservationRequest model)
        {
            model.UserId = UserId;
            Facade.Command(model);
            return CreateOkHttpResponse();
        }

        [Route("Message")]
        [HttpPost]
        public IActionResult Message([FromForm]MessageReservationRequest model)
        {
            model.MessageSentBy = UserId;
            Facade.Command(model);
            return CreateOkHttpResponse();
        }

        [Route("Metrics")]
        [HttpPost]
        public IActionResult Metrics([FromForm]GetMetricsRequest model)
        {
            model.UserId = UserId;
            var response = Facade.Query<GetMetricsResponse, GetMetricsRequest>(model);
            return CreateHttpResponse(response);
        }

        [Route("GetMessages")]
        [HttpPost]
        public IActionResult GetMessages([FromForm]GetMessagesRequest model)
        {
            model.UserId = UserId;
            var response = Facade.Query<GetMessagesResponse, GetMessagesRequest>(model);
            return CreateHttpResponse(response);
        }

        [Route("GetSummary")]
        [HttpPost]
        public IActionResult GetSummary([FromForm]GetSummaryRequest model)
        {
            model.UserId = UserId;
            var response = Facade.Query<GetSummaryResponse, GetSummaryRequest>(model);
            return CreateHttpResponse(response);
        }

        [Route("GetTimeSlots")]
        [HttpPost]
        public IActionResult GetTimeSlots([FromForm]GetTimeSlotsRequest model)
        {
            model.UserId = UserId;
            var response = Facade.Query<GetTimeSlotsResponse, GetTimeSlotsRequest>(model);
            return CreateHttpResponse(response);
        }

        [AllowAnonymous]
        [Route("business/{id}")]
        [HttpPost]
        public IActionResult GetReservationBusinessInfo([FromForm]GetReservationBusinessByIdRequest model)
        {
            var response = Facade.Query<GetReservationBusinessByIdResponse, GetReservationBusinessByIdRequest>(model);
            return CreateHttpResponse(response);
        }

        [AllowAnonymous]
        [Route("AddReservation")]
        [HttpPost]
        public async Task<IActionResult> AddReservation([FromForm]AddReservationRequest model)
        {
            var response = await Facade.CommandAsync<AddReservationRequest, AddReservationResponse>(model);
            return CreateHttpResponse(response);
        }

    }
}
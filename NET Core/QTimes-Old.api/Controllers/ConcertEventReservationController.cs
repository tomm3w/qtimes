using iVeew.common.api;
using iVeew.common.api.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QTimes.api.Commands.Reservation.AddConcertEventReservation;
using QTimes.api.Commands.Reservation.LockConcertEventSeat;
using QTimes.api.Commands.Reservation.MessageConcertEventReservation;
using QTimes.api.Commands.Reservation.UpdateConcertEventCheckIn;
using QTimes.api.Commands.Reservation.UpdateConcertEventReservation;
using QTimes.api.Infrastructure;
using QTimes.api.Queries.Reservation.GetConcertById;
using QTimes.api.Queries.Reservation.GetConcertEventMessages;
using QTimes.api.Queries.Reservation.GetConcertEventReservations;
using QTimes.api.Queries.Reservation.GetConcertMetrics;
using System;
using System.Threading.Tasks;

namespace QTimes.api.Controllers
{
    //[Authorize]
    //[ValidateModel]
    [ApiController]
    [Route("[controller]")]
    public class ConcertEventReservationController : SpecificApiController
    {
        public ConcertEventReservationController(IApplicationFacade applicationFacade)
           : base(applicationFacade)
        { }

        [Route("/concerts/{concertId:Guid}/reservations")]
        [HttpGet]
        public IActionResult Get([FromQuery] int Page, [FromQuery] int PageSize, Guid concertId, [FromQuery] Guid? concertEventId, string SearchText = null)
        {
            var model = new GetConcertEventReservationsRequest
            {
                ConcertId = concertId,
                UserId = UserId,
                Page = Page,
                PageSize = PageSize,
                SearchText = SearchText,
                ConcertEventId = concertEventId,
            };

            var response = Facade.Query<GetConcertEventReservationsResponse, GetConcertEventReservationsRequest>(model);
            return CreateHttpResponse(response);
        }

        //TODO: re-use for mobile scenario
        [Route("/concerts/{concertId:Guid}/events/{eventId:Guid}/reservations")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Post([FromForm] AddConcertEventReservationRequest request, Guid concertId, Guid eventId)
        {
            request.ConcertEventId = eventId;
            var response = await Facade.CommandAsync<AddConcertEventReservationRequest, AddConcertEventReservationResponse>(request);
            return CreateHttpResponse(response);
        }

        [Route("/concerts/{concertId:Guid}/events/{eventId:Guid}/reservations/{id:int}")]
        [HttpPut]
        public IActionResult Put([FromForm] UpdateConcertEventReservationRequest request, Guid concertId, Guid eventId, int id)
        {
            request.Id = id;
            Facade.Command(request);
            return CreateOkHttpResponse();
        }


        [Route("/concerts/{concertId:Guid}/events/{eventId:Guid}/messages")]
        [HttpPost]
        public IActionResult ConcertMessage([FromForm] MessageConcertEventReservationRequest request, Guid concertId, Guid eventId)
        {
            request.MessageSentBy = UserId;
            Facade.Command(request);
            return CreateOkHttpResponse();
        }

        [Route("/concerts/{concertId:Guid}/metrics")]
        [HttpPost]
        public IActionResult Metrics([FromForm] GetConcertMetricsRequest request, Guid concertId, [FromQuery] Guid? concertEventId)
        {
            request.UserId = UserId;
            request.ConcertEventId = concertEventId;
            var response = Facade.Query<GetConcertMetricsResponse, GetConcertMetricsRequest>(request);
            return CreateHttpResponse(response);
        }

        [Route("/concerts/{concertId:Guid}/events/{eventId:Guid}/reservations/{reservationId:int}/messages")]
        [HttpGet]
        public IActionResult GetConcertEventReservationMessages(Guid concertId, Guid eventId, int reservationId)
        {
            GetConcertEventMessagesRequest request = new GetConcertEventMessagesRequest();
            request.UserId = UserId;
            request.ConcertEventReseravationId = reservationId;
            var response = Facade.Query<GetConcertEventMessagesResponse, GetConcertEventMessagesRequest>(request);
            return CreateHttpResponse(response);
        }

        //TODO: Require separate implementatin for concert case
        //[AllowAnonymous]
        //[Route("AddReservation")]
        //[HttpPost]
        //public async Task<IActionResult> AddReservation([FromForm] AddReservationRequest request)
        //{
        //    await Facade.CommandAsync<AddReservationRequest, string>(request);
        //    return CreateOkHttpResponse();
        //}


        [AllowAnonymous]
        [Route("/concerts/{id:Guid}")]
        [HttpGet]
        public IActionResult GetConcert(Guid id)
        {
            var response = Facade.Query<GetConcertByIdResponse, GetConcertByIdRequest>(new GetConcertByIdRequest { Id = id });
            return CreateHttpResponse(response);
        }

        //[AllowAnonymous]
        //[Route("addconcertreservation")]
        //[HttpPost]
        //public IActionResult AddConcertReservation([FromForm] AddConcertEventReservationRequest model)
        //{
        //    model.GuestTypeId = (int)GuestTypeEnum.MobileConnected;
        //    Facade.Command(model);
        //    return CreateOkHttpResponse();
        //}

        [AllowAnonymous]
        [Route("/concerts/{concertId:Guid}/events/{concertEventId:Guid}/checkin")]
        [HttpGet]
        public IActionResult GetConcertEventCheckInGuest(int Page, int PageSize, Guid concertId, Guid concertEventId, string SearchText = null)
        {
            var model = new GetConcertEventReservationsRequest
            {
                Page = Page,
                PageSize = PageSize,
                SearchText = SearchText,
                ConcertEventId = concertEventId,
                ConcertId = concertId
            };

            var response = Facade.Query<GetConcertEventReservationsResponse, GetConcertEventReservationsRequest>(model);
            return CreateHttpResponse(response);
        }

        [AllowAnonymous]
        [Route("/concerts/{concertId:Guid}/events/{:concertEventId}/checkin")]
        [HttpPut]
        public IActionResult UpdateConcertEventCheckIn([FromForm] UpdateConcertEventCheckInRequest request)
        {
            Facade.Command(request);
            return CreateOkHttpResponse();
        }

        [AllowAnonymous]
        [Route("/concerts/{concertId:Guid}/events/{eventId:Guid}/seatlocks/{seatNo}")]
        [HttpGet]
        public IActionResult ValidateSeatLock(Guid concertId, Guid eventId, string seatNo)
        {
            LockConcertEventSeatRequest request = new LockConcertEventSeatRequest()
            {
                ConcertEventId = eventId,
                SeatNo = seatNo
            };
            Facade.Command(request);
            return CreateOkHttpResponse();
        }
    }
}
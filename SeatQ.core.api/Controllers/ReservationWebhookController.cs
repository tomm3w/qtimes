using common.api;
using common.api.Infrastructure;
using SeatQ.core.api.Commands.Reservation.DeliveryReceiptWebhook;
using SeatQ.core.api.Commands.Reservation.InboundWebhook;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace SeatQ.core.api.Controllers
{
    [RoutePrefix("api/reservationwebhook")]
    [EnableCors(origins: "*", headers: "*", methods: "*", SupportsCredentials = true)]
    public class ReservationWebhookController : SpecificApiController
    {
        public ReservationWebhookController(IApplicationFacade applicationFacade)
           : base(applicationFacade)
        { }

        [HttpPost]
        [Route("inbound-sms")]
        public HttpResponseMessage inbound([FromBody] InboundWebhookRequest model)
        {
            if (model != null)
                Facade.Command(model);
            return new HttpResponseMessage(HttpStatusCode.NoContent);
        }

        [HttpGet]
        [Route("delivery-receipt")]
        public HttpResponseMessage deliverystatus([FromUri] DeliveryReceiptWebhookRequest model)
        {
            if (model != null)
                Facade.Command(model);
            return new HttpResponseMessage(HttpStatusCode.NoContent);
        }
    }
}

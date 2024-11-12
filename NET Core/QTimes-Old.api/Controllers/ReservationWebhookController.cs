using iVeew.common.api;
using iVeew.common.api.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using QTimes.api.Commands.Reservation.InboundWebhook;
using QTimes.api.Commands.Reservation.DeliveryReceiptWebhook;
using Microsoft.Extensions.Logging;

namespace QTimes.api.Controllers
{
    //[RoutePrefix("api/reservationwebhook")]
    //[EnableCors("MyPolicy")]
    [ApiController]
    [Route("[controller]")]
    public class ReservationWebhookController : SpecificApiController
    {
        private readonly ILogger<ReservationWebhookController> _logger;
        public ReservationWebhookController(IApplicationFacade applicationFacade, ILogger<ReservationWebhookController> logger)
           : base(applicationFacade)
        {
            _logger = logger;
        }

        [HttpGet]
        [Route("inbound-sms")]
        public IActionResult inbound([FromQuery] InboundWebhookRequest model)
        {
            _logger.LogInformation($"Nexmo inbound call {model.Text}");
            if (model != null)
                Facade.Command(model);
            return NoContent();
        }

        [HttpGet]
        [Route("delivery-receipt")]
        public IActionResult deliverystatus([FromQuery] DeliveryReceiptWebhookRequest model)
        {
            if (model != null)
                Facade.Command(model);
            return NoContent();
        }
    }
}

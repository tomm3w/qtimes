using common.api.Commands;
using Nexmo.Api.Messaging;

namespace SeatQ.core.api.Commands.Reservation.InboundWebhook
{
    public class InboundWebhookRequest : InboundSms, ICommandRequest
    {
    }
}
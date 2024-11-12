using iVeew.common.api.Commands;
using Nexmo.Api.Messaging;

namespace QTimes.api.Commands.Reservation.InboundWebhook
{
    public class InboundWebhookRequest : InboundSms, ICommandRequest
    {
    }
}
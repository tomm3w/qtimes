using common.api.Commands;
using Nexmo.Api.Messaging;

namespace SeatQ.core.api.Commands.Reservation.DeliveryReceiptWebhook
{
    public class DeliveryReceiptWebhookRequest : DeliveryReceipt, ICommandRequest
    {
    }
}
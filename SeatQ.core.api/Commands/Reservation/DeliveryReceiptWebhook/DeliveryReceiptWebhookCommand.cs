using common.api.Commands;
using common.dal;
using SeatQ.core.dal.Models;
using System;

namespace SeatQ.core.api.Commands.Reservation.DeliveryReceiptWebhook
{
    public class DeliveryReceiptWebhookCommand : ICommand<DeliveryReceiptWebhookRequest>
    {
        private readonly IGenericRepository<SeatQEntities, DeliveryStatu> _deliveryStatusRepository;

        public DeliveryReceiptWebhookCommand(IGenericRepository<SeatQEntities, DeliveryStatu> deliveryStatusRepository)
        {
            _deliveryStatusRepository = deliveryStatusRepository;
        }

        public void Handle(DeliveryReceiptWebhookRequest request)
        {
            var message = new DeliveryStatu
            {
                msisdn = request.Msisdn,
                to = request.To,
                network_code = request.NetworkCode,
                messageId = request.MessageId,
                price = request.Price,
                status = request.StringStatus,
                scts = request.Scts,
                err_code = request.ErrorCode,
                message_timestamp = Convert.ToDateTime(request.MessageTimestamp),
                Project = "qtimes"
            };

            _deliveryStatusRepository.Add(message);
            _deliveryStatusRepository.Save();

        }
    }
}
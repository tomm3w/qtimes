using iVeew.common.api.Commands;
using IVeew.common.dal;
using QTimes.core.dal.Models;
using System;

namespace QTimes.api.Commands.Reservation.DeliveryReceiptWebhook
{
    public class DeliveryReceiptWebhookCommand : ICommand<DeliveryReceiptWebhookRequest>
    {
        private readonly IGenericRepository<QTimesContext, DeliveryStatus> _deliveryStatusRepository;

        public DeliveryReceiptWebhookCommand(IGenericRepository<QTimesContext, DeliveryStatus> deliveryStatusRepository)
        {
            _deliveryStatusRepository = deliveryStatusRepository;
        }

        public void Handle(DeliveryReceiptWebhookRequest request)
        {
            var message = new DeliveryStatus
            {
                Msisdn = request.Msisdn,
                To = request.To,
                NetworkCode = request.NetworkCode,
                MessageId = request.MessageId,
                Price = request.Price,
                Status = request.StringStatus,
                Scts = request.Scts,
                ErrCode = request.ErrorCode,
                MessageTimestamp = Convert.ToDateTime(request.MessageTimestamp),
                Project = "qtimes"
            };

            _deliveryStatusRepository.Add(message);
            _deliveryStatusRepository.Save();

        }
    }
}
using iVeew.common.api.Commands;
using IVeew.common.dal;
using QTimes.core.dal.Models;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace QTimes.api.Commands.Reservation.InboundWebhook
{
    public class InboundWebhookCommand : ICommand<InboundWebhookRequest>
    {
        private readonly IGenericRepository<QTimesContext, core.dal.Models.Reservation> _reservationRepository;
        private readonly IGenericRepository<QTimesContext, ReservationMessage> _reservationMessageRepository;
        public InboundWebhookCommand(IGenericRepository<QTimesContext, core.dal.Models.Reservation> reservationRepository,
            IGenericRepository<QTimesContext, ReservationMessage> reservationMessageRepository)
        {
            _reservationRepository = reservationRepository;
            _reservationMessageRepository = reservationMessageRepository;
        }

        public void Handle(InboundWebhookRequest request)
        {
            request.Msisdn = Regex.Replace(request.Msisdn, "[^0-9]", "");
            var reservations = _reservationRepository.FindBy(x => x.BusinessDetail.VirtualNo == request.To && x.ReservationGuest.MobileNumber == request.Msisdn)
                .OrderByDescending(x => x.CreatedDateTime);

            if (reservations.Any())
            {
                var reservation = reservations.First();

                var message = new ReservationMessage
                {
                    ReservationId = reservation.Id,
                    MessageReplied = request.Text,
                    MessageRepliedDateTime = DateTime.UtcNow,
                    IsRead = false,
                    MessageSentBy = reservation.ReservationGuestId,
                    MessageSentDateTime = DateTime.UtcNow
                };

                _reservationMessageRepository.Add(message);
                _reservationMessageRepository.Save();
            }
        }
    }
}
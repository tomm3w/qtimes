using iVeew.common.api.Commands;
using IVeew.common.dal;
using QTimes.api.Infrastructure.NexmoMessaging;
using QTimes.core.dal.Models;
using System;
using System.Linq;

namespace QTimes.api.Commands.Reservation.MessageReservation
{
    public class MessageReservationCommand : ICommand<MessageReservationRequest>
    {
        private readonly IGenericRepository<QTimesContext, ReservationMessage> _reservationMessageRepository;
        private readonly IGenericRepository<QTimesContext, core.dal.Models.Reservation> _reservationRepository;
        private readonly IMessaging _messaging;
        public MessageReservationCommand(IGenericRepository<QTimesContext, ReservationMessage> reservationMessageRepository,
            IGenericRepository<QTimesContext, core.dal.Models.Reservation> reservationRepository,
            IMessaging messaging)
        {
            _reservationRepository = reservationRepository;
            _reservationMessageRepository = reservationMessageRepository;
            _messaging = messaging;
        }

        public void Handle(MessageReservationRequest request)
        {
            var reserve = _reservationRepository.FindBy(x => x.Id == request.Id).FirstOrDefault();
            if (reserve == null)
                throw new Exception("Reservation not found.");

            var from = reserve.BusinessDetail.VirtualNo;
            var to = reserve.ReservationGuest.MobileNumber;

            if (from == null)
                throw new Exception("Virtual number not found.");

            if (to == null)
                throw new Exception("Guest number not found.");

            var response = _messaging.Send(from, to, request.MessageSent);

            _reservationMessageRepository.Add(new ReservationMessage
            {
                ReservationId = request.Id,
                MessageSent = request.MessageSent,
                MessageSentBy = request.MessageSentBy,
                MessageSentDateTime = DateTime.UtcNow
            });

            _reservationMessageRepository.Save();
        }

    }
}
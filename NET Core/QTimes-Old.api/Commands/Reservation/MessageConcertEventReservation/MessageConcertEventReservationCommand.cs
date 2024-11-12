using iVeew.common.api.Commands;
using IVeew.common.dal;
using QTimes.api.Infrastructure.NexmoMessaging;
using QTimes.core.dal.Models;
using System;
using System.Linq;

namespace QTimes.api.Commands.Reservation.MessageConcertEventReservation
{
    public class MessageConcertEventReservationCommand : ICommand<MessageConcertEventReservationRequest>
    {
        private readonly IGenericRepository<QTimesContext, ConcertEventReservationMessage> _reservationMessageRepository;
        private readonly IGenericRepository<QTimesContext, ConcertEventReservation> _reservationRepository;
        private readonly IMessaging _messaging;
        public MessageConcertEventReservationCommand(IGenericRepository<QTimesContext, ConcertEventReservationMessage> reservationMessageRepository,
            IGenericRepository<QTimesContext, ConcertEventReservation> reservationRepository,
            IMessaging messaging)
        {
            _reservationRepository = reservationRepository;
            _reservationMessageRepository = reservationMessageRepository;
            _messaging = messaging;
        }

        public void Handle(MessageConcertEventReservationRequest request)
        {
            var reserve = _reservationRepository.FindBy(x => x.Id == request.Id).FirstOrDefault();
            if (reserve == null)
                throw new Exception("Reservation not found.");

            var from = reserve.ConcertEvent.Concert.VirtualNo;
            var to = reserve.ConcertEventGuest.FirstOrDefault(x => x.IsMainGuest == true).MobileNo;

            if (from == null)
                throw new Exception("Virtual number not found.");

            if (to == null)
                throw new Exception("Guest number not found.");

            var response = _messaging.Send(from, to, request.MessageSent);

            _reservationMessageRepository.Add(new ConcertEventReservationMessage
            {
                ConcertEventReservationId = request.Id,
                MessageSent = request.MessageSent,
                MessageSentBy = request.MessageSentBy,
                MessageSentDateTime = DateTime.UtcNow,
            });

            _reservationMessageRepository.Save();
        }

    }
}
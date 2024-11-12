using common.api.Commands;
using common.dal;
using SeatQ.core.api.Infrastructure.NexmoMessaging;
using SeatQ.core.dal.Models;
using System;
using System.Linq;

namespace SeatQ.core.api.Commands.Reservation.MessageConcertReservation
{
    public class MessageConcertReservationCommand : ICommand<MessageConcertReservationRequest>
    {
        private readonly IGenericRepository<SeatQEntities, ConcertReservationMessage> _reservationMessageRepository;
        private readonly IGenericRepository<SeatQEntities, dal.Models.ConcertReservation> _reservationRepository;
        private readonly IMessaging _messaging;
        public MessageConcertReservationCommand(IGenericRepository<SeatQEntities, ConcertReservationMessage> reservationMessageRepository,
            IGenericRepository<SeatQEntities, dal.Models.ConcertReservation> reservationRepository,
            IMessaging messaging)
        {
            _reservationRepository = reservationRepository;
            _reservationMessageRepository = reservationMessageRepository;
            _messaging = messaging;
        }

        public void Handle(MessageConcertReservationRequest request)
        {
            var reserve = _reservationRepository.FindBy(x => x.Id == request.Id).FirstOrDefault();
            if (reserve == null)
                throw new Exception("Reservation not found.");

            var from = reserve.Concert.VirtualNo;
            var to = reserve.ConcertGuests.FirstOrDefault(x => x.IsMainGuest == true).MobileNo;

            if (from == null)
                throw new Exception("Virtual number not found.");

            if (to == null)
                throw new Exception("Guest number not found.");

            var response = _messaging.Send(from, to, request.MessageSent);

            _reservationMessageRepository.Add(new ConcertReservationMessage
            {
                ConcertReservationId = request.Id,
                MessageSent = request.MessageSent,
                MessageSentBy = request.MessageSentBy,
                MessageSentDateTime = DateTime.UtcNow,
            });

            _reservationMessageRepository.Save();
        }

    }
}
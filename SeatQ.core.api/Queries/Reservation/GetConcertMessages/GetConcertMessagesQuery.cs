using common.api.Queries;
using common.dal;
using SeatQ.core.dal.Models;
using System.Linq;

namespace SeatQ.core.api.Queries.Reservation.GetConcertMessages
{
    public class GetConcertMessagesQuery : IQuery<GetConcertMessagesResponse, GetConcertMessagesRequest>
    {
        private readonly IGenericRepository<SeatQEntities, ConcertReservationMessage> _repository;
        public GetConcertMessagesQuery(IGenericRepository<SeatQEntities, ConcertReservationMessage> repository)
        {
            _repository = repository;
        }
        public GetConcertMessagesResponse Handle(GetConcertMessagesRequest request)
        {
            var messages = _repository.FindBy(x => x.ConcertReservationId == request.ConcertReservationId);
            var data = messages.Select(x => new ConcertMessages
            {
                Id = x.Id,
                ConcertReservationId = x.ConcertReservationId,
                MessageSent = x.MessageSent,
                MessageSentDateTime = x.MessageSentDateTime,
                MessageSentBy = x.MessageSentBy,
                MessageReplied = x.MessageReplied,
                MessageRepliedDateTime = x.MessageRepliedDateTime,
                GuestName = x.ConcertReservation.ConcertGuests.FirstOrDefault(y => y.IsMainGuest == true).Name
            });
            return new GetConcertMessagesResponse { Model = data.ToList() };
        }

    }
}
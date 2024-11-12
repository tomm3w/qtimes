using iVeew.common.api.Queries;
using IVeew.common.dal;
using QTimes.core.dal.Models;
using System.Linq;

namespace QTimes.api.Queries.Reservation.GetConcertEventMessages
{
    public class GetConcertEventMessagesQuery : IQuery<GetConcertEventMessagesResponse, GetConcertEventMessagesRequest>
    {
        private readonly IGenericRepository<QTimesContext, ConcertEventReservationMessage> _repository;
        public GetConcertEventMessagesQuery(IGenericRepository<QTimesContext, ConcertEventReservationMessage> repository)
        {
            _repository = repository;
        }
        public GetConcertEventMessagesResponse Handle(GetConcertEventMessagesRequest request)
        {
            var messages = _repository.FindBy(x => x.ConcertEventReservationId == request.ConcertEventReseravationId);
            var data = messages.Select(x => new ConcertMessages
            {
                Id = x.Id,
                ConcertReservationId = x.ConcertEventReservationId,
                MessageSent = x.MessageSent,
                MessageSentDateTime = x.MessageSentDateTime,
                MessageSentBy = x.MessageSentBy,
                MessageReplied = x.MessageReplied,
                MessageRepliedDateTime = x.MessageRepliedDateTime,
                GuestName = x.ConcertEventReservation.ConcertEventGuest.FirstOrDefault(y => y.IsMainGuest == true).Name
            });
            return new GetConcertEventMessagesResponse { Model = data.ToList() };
        }

    }
}
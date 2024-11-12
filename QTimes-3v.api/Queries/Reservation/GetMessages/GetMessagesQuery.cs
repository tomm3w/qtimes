using AutoMapper;
using iVeew.common.api.Queries;
using IVeew.common.dal;
using QTimes.core.dal.Models;
using System.Collections.Generic;

namespace QTimes.api.Queries.Reservation.GetMessages
{
    public class GetMessagesQuery : IQuery<GetMessagesResponse, GetMessagesRequest>
    {
        private readonly IGenericRepository<QTimesContext, ReservationMessage> _repository;
        public GetMessagesQuery(IGenericRepository<QTimesContext, ReservationMessage> repository)
        {
            _repository = repository;
        }
        public GetMessagesResponse Handle(GetMessagesRequest request)
        {
            var messages = _repository.FindBy(x => x.ReservationId == request.ReservationId);
            var data = Mapper.Map<List<Messages>>(messages);
            return new GetMessagesResponse { Model = data };
        }

    }
}
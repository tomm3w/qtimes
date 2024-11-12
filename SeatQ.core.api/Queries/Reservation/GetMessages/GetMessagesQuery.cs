using AutoMapper;
using common.api.Queries;
using common.dal;
using SeatQ.core.dal.Models;
using System.Collections.Generic;

namespace SeatQ.core.api.Queries.Reservation.GetMessages
{
    public class GetMessagesQuery : IQuery<GetMessagesResponse, GetMessagesRequest>
    {
        private readonly IGenericRepository<SeatQEntities, ReservationMessage> _repository;
        public GetMessagesQuery(IGenericRepository<SeatQEntities, ReservationMessage> repository)
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
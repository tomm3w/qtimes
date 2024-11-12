using AutoMapper;
using common.api.Queries;
using common.dal;
using SeatQ.core.dal.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SeatQ.core.api.Queries.Reservation.GetReservationMessageRule
{
    public class GetReservationMessageRuleQuery : IQuery<GetReservationMessageRuleResponse, GetReservationMessageRuleRequest>
    {
        private readonly IGenericRepository<SeatQEntities, ReservationMessageRule> _reservationRepository;
        private readonly IGenericRepository<SeatQEntities, UserInfo> _userInfoRepository;

        public GetReservationMessageRuleQuery(IGenericRepository<SeatQEntities, ReservationMessageRule> reservationRepository,
            IGenericRepository<SeatQEntities, UserInfo> userInfoRepository)
        {
            _reservationRepository = reservationRepository;
            _userInfoRepository = userInfoRepository;
        }

        public GetReservationMessageRuleResponse Handle(GetReservationMessageRuleRequest request)
        {
              var msgs = _reservationRepository.FindBy(x => x.BusinessDetailId == request.BusinessDetailId).OrderByDescending(x => x.MessageType);

            var data = Mapper.Map<List<MessageRules>>(msgs);

            return new GetReservationMessageRuleResponse
            {
                Model = data
            };
        }

    }
}
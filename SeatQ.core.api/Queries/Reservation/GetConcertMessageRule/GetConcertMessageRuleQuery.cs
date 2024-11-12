using AutoMapper;
using common.api.Queries;
using common.dal;
using SeatQ.core.dal.Models;
using System.Collections.Generic;
using System.Linq;

namespace SeatQ.core.api.Queries.Reservation.GetConcertMessageRule
{
    public class GetConcertMessageRuleQuery : IQuery<GetConcertMessageRuleResponse, GetConcertMessageRuleRequest>
    {
        private readonly IGenericRepository<SeatQEntities, ConcertMessageRule> _reservationRepository;
        private readonly IGenericRepository<SeatQEntities, UserInfo> _userInfoRepository;

        public GetConcertMessageRuleQuery(IGenericRepository<SeatQEntities, ConcertMessageRule> reservationRepository,
            IGenericRepository<SeatQEntities, UserInfo> userInfoRepository)
        {
            _reservationRepository = reservationRepository;
            _userInfoRepository = userInfoRepository;
        }

        public GetConcertMessageRuleResponse Handle(GetConcertMessageRuleRequest request)
        {
            var msgs = _reservationRepository.FindBy(x => x.ConcertId == request.ConcertId).OrderByDescending(x => x.MessageType);

            var data = Mapper.Map<List<ConcertMessageRules>>(msgs);

            return new GetConcertMessageRuleResponse
            {
                Model = data
            };
        }

    }
}
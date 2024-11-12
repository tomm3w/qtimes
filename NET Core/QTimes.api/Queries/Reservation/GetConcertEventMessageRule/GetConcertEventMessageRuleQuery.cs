using AutoMapper;
using iVeew.common.api.Queries;
using IVeew.common.dal;
using QTimes.core.dal.Models;
using System.Collections.Generic;
using System.Linq;

namespace QTimes.api.Queries.Reservation.GetConcertEventMessageRule
{
    public class GetConcertEventMessageRuleQuery : IQuery<GetConcertEventMessageRuleResponse, GetConcertEventMessageRuleRequest>
    {
        private readonly IGenericRepository<QTimesContext, ConcertEventMessageRule> _reservationRepository;

        public GetConcertEventMessageRuleQuery(IGenericRepository<QTimesContext, ConcertEventMessageRule> reservationRepository)
        {
            _reservationRepository = reservationRepository;
        }

        public GetConcertEventMessageRuleResponse Handle(GetConcertEventMessageRuleRequest request)
        {
            var msgs = _reservationRepository.FindBy(x => x.ConcertEventId == request.ConcertEventId).OrderByDescending(x => x.MessageType);

            var data = Mapper.Map<List<ConcertMessageRules>>(msgs);

            return new GetConcertEventMessageRuleResponse
            {
                Model = data
            };
        }

    }
}
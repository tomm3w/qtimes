using AutoMapper;
using iVeew.common.api.Queries;
using IVeew.common.dal;
using QTimes.core.dal.Models;
using System.Collections.Generic;
using System.Linq;

namespace QTimes.api.Queries.Reservation.GetReservationMessageRule
{
    public class GetReservationMessageRuleQuery : IQuery<GetReservationMessageRuleResponse, GetReservationMessageRuleRequest>
    {
        private readonly IGenericRepository<QTimesContext, ReservationMessageRule> _reservationRepository;
        private readonly IGenericRepository<QTimesContext, UserInfo> _userInfoRepository;

        public GetReservationMessageRuleQuery(IGenericRepository<QTimesContext, ReservationMessageRule> reservationRepository,
            IGenericRepository<QTimesContext, UserInfo> userInfoRepository)
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
using iVeew.common.api.Commands;
using IVeew.common.dal;
using QTimes.core.dal.Models;
using System.Linq;

namespace QTimes.api.Commands.Reservation.DeleteReservationMessageRule
{
    public class DeleteReservationMessageRuleCommand : ICommand<DeleteReservationMessageRuleRequest>
    {
        private readonly IGenericRepository<QTimesContext, ReservationMessageRule> _ruleRepository;
        private readonly IGenericRepository<QTimesContext, UserInfo> _userInfoRepository;

        public DeleteReservationMessageRuleCommand(IGenericRepository<QTimesContext, ReservationMessageRule> ruleRepository,
            IGenericRepository<QTimesContext, UserInfo> userInfoRepository)
        {
            _ruleRepository = ruleRepository;
            _userInfoRepository = userInfoRepository;
        }

        public void Handle(DeleteReservationMessageRuleRequest request)
        {

            var rule = _ruleRepository.FindBy(x => x.Id == request.Id);
            if(rule.Any())
            {
                _ruleRepository.Delete(rule.FirstOrDefault());
            }
            
            _ruleRepository.Save();
        }
    }
}
using iVeew.common.api.Commands;
using IVeew.common.dal;
using QTimes.core.dal.Models;
using System.Linq;

namespace QTimes.api.Commands.Reservation.DeleteConcertEventMessageRule
{
    public class DeleteConcertEventMessageRule : ICommand<DeleteConcertEventMessageRuleRequest>
    {
        private readonly IGenericRepository<QTimesContext, ConcertEventMessageRule> _ruleRepository;

        public DeleteConcertEventMessageRule(IGenericRepository<QTimesContext, ConcertEventMessageRule> ruleRepository)
        {
            _ruleRepository = ruleRepository;
        }

        public void Handle(DeleteConcertEventMessageRuleRequest request)
        {
            var rule = _ruleRepository.FindBy(x => x.Id == request.Id && x.ConcertEventId == request.ConcertEventId);
            if (rule.Any())
            {
                _ruleRepository.Delete(rule.FirstOrDefault());
            }

            _ruleRepository.Save();
        }
    }
}
using iVeew.common.api.Commands;
using IVeew.common.dal;
using QTimes.core.dal.Models;
using System;
using System.Linq;

namespace QTimes.api.Commands.Reservation.UpdateConcertEventMessageRule
{
    public class UpdateConcertEventMessageRuleCommand : ICommand<UpdateConcertEventMessageRuleRequest>
    {
        private readonly IGenericRepository<QTimesContext, ConcertEventMessageRule> _ruleRepository;

        public UpdateConcertEventMessageRuleCommand(IGenericRepository<QTimesContext, ConcertEventMessageRule> ruleRepository)
        {
            _ruleRepository = ruleRepository;
        }

        public void Handle(UpdateConcertEventMessageRuleRequest request)
        {
            var msg = _ruleRepository.FindBy(x => x.Id == request.Id).FirstOrDefault();
            if (msg == null)
                throw new Exception("Concert message not found.");

            msg.BeforeAfter = request.BeforeAfter;
            msg.InOut = request.InOut;
            msg.Message = request.Message;
            msg.MessageType = request.MessageType;
            msg.Value = request.Value;
            msg.ValueType = request.ValueType;
            _ruleRepository.Save();
        }

    }
}
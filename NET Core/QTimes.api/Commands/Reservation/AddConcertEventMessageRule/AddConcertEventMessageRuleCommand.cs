using iVeew.common.api.Commands;
using IVeew.common.dal;
using QTimes.core.dal.Models;
using System;
using System.Linq;

namespace QTimes.api.Commands.Reservation.AddConcertEventMessageRule
{
    public class AddConcertEventMessageRuleCommand : ICommand<AddConcertEventMessageRuleRequest>
    {
        private readonly IGenericRepository<QTimesContext, ConcertEventMessageRule> _ruleRepository;
        
        public AddConcertEventMessageRuleCommand(IGenericRepository<QTimesContext, ConcertEventMessageRule> ruleRepository)
        {
            _ruleRepository = ruleRepository;
        }

        public void Handle(AddConcertEventMessageRuleRequest request)
        {

            if(request.MessageType.Equals("Welcome"))
            {
                var exist = _ruleRepository.FindBy(x => x.ConcertEventId == request.ConcertEventId&& x.MessageType.Equals("Welcome"));
                if (exist.Any())
                    throw new Exception("Welcome message already exists.");
            }

            _ruleRepository.Add(new ConcertEventMessageRule
            {
                BeforeAfter = request.BeforeAfter,
                InOut = request.InOut,
                Message = request.Message,
                MessageType = request.MessageType,
                ConcertEventId= request.ConcertEventId,
                Value = request.Value ?? 0,
                ValueType = request.ValueType
            });

            _ruleRepository.Save();
        }
    }
}
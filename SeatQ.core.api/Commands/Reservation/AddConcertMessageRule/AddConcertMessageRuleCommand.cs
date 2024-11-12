using common.api.Commands;
using common.dal;
using SeatQ.core.dal.Models;
using System;
using System.Linq;

namespace SeatQ.core.api.Commands.Reservation.AddConcertMessageRule
{
    public class AddConcertMessageRuleCommand : ICommand<AddConcertMessageRuleRequest>
    {
        private readonly IGenericRepository<SeatQEntities, ConcertMessageRule> _reservationRepository;
        private readonly IGenericRepository<SeatQEntities, UserInfo> _userInfoRepository;

        public AddConcertMessageRuleCommand(IGenericRepository<SeatQEntities, ConcertMessageRule> reservationRepository,
            IGenericRepository<SeatQEntities, UserInfo> userInfoRepository)
        {
            _reservationRepository = reservationRepository;
            _userInfoRepository = userInfoRepository;
        }

        public void Handle(AddConcertMessageRuleRequest request)
        {

            if(request.MessageType.Equals("Welcome"))
            {
                var exist = _reservationRepository.FindBy(x => x.ConcertId == request.ConcertId && x.MessageType.Equals("Welcome"));
                if (exist.Any())
                    throw new Exception("Welcome message already defined.");
            }

            _reservationRepository.Add(new ConcertMessageRule
            {
                BeforeAfter = request.BeforeAfter,
                InOut = request.InOut,
                Message = request.Message,
                MessageType = request.MessageType,
                ConcertId = request.ConcertId,
                Value = request.Value ?? 0,
                ValueType = request.ValueType
            });

            _reservationRepository.Save();
        }
    }
}
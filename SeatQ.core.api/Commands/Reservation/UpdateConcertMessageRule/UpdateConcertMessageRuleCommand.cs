using common.api.Commands;
using common.dal;
using SeatQ.core.dal.Models;
using System;
using System.Linq;

namespace SeatQ.core.api.Commands.Reservation.UpdateReservationMessageRule
{
    public class UpdateConcertMessageRuleCommand : ICommand<UpdateConcertMessageRuleRequest>
    {
        private readonly IGenericRepository<SeatQEntities, ConcertMessageRule> _reservationRepository;
        private readonly IGenericRepository<SeatQEntities, UserInfo> _userInfoRepository;

        public UpdateConcertMessageRuleCommand(IGenericRepository<SeatQEntities, ConcertMessageRule> reservationRepository,
            IGenericRepository<SeatQEntities, UserInfo> userInfoRepository)
        {
            _reservationRepository = reservationRepository;
            _userInfoRepository = userInfoRepository;
        }

        public void Handle(UpdateConcertMessageRuleRequest request)
        {
            var msg = _reservationRepository.FindBy(x => x.Id == request.Id).FirstOrDefault();
            if (msg == null)
                throw new Exception("Concert message not found.");

            msg.BeforeAfter = request.BeforeAfter;
            msg.InOut = request.InOut;
            msg.Message = request.Message;
            msg.MessageType = request.MessageType;
            msg.Value = request.Value;
            msg.ValueType = request.ValueType;
            _reservationRepository.Save();
        }

    }
}
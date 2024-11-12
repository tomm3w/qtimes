using iVeew.common.api.Commands;
using IVeew.common.dal;
using QTimes.core.dal.Models;
using System;
using System.Linq;

namespace QTimes.api.Commands.Reservation.UpdateReservationMessageRule
{
    public class UpdateReservationMessageRuleCommand : ICommand<UpdateReservationMessageRuleRequest>
    {
        private readonly IGenericRepository<QTimesContext, ReservationMessageRule> _reservationRepository;
        private readonly IGenericRepository<QTimesContext, UserInfo> _userInfoRepository;

        public UpdateReservationMessageRuleCommand(IGenericRepository<QTimesContext, ReservationMessageRule> reservationRepository,
            IGenericRepository<QTimesContext, UserInfo> userInfoRepository)
        {
            _reservationRepository = reservationRepository;
            _userInfoRepository = userInfoRepository;
        }

        public void Handle(UpdateReservationMessageRuleRequest request)
        {
            var msg = _reservationRepository.FindBy(x => x.Id == request.Id).FirstOrDefault();
            if (msg == null)
                throw new Exception("Reservation message not found.");

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
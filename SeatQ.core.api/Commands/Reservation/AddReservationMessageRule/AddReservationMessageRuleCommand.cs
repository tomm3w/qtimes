using common.api.Commands;
using common.dal;
using SeatQ.core.dal.Models;
using System;
using System.Linq;

namespace SeatQ.core.api.Commands.Reservation.AddReservationMessageRule
{
    public class AddReservationMessageRuleCommand : ICommand<AddReservationMessageRuleRequest>
    {
        private readonly IGenericRepository<SeatQEntities, ReservationMessageRule> _reservationRepository;
        private readonly IGenericRepository<SeatQEntities, UserInfo> _userInfoRepository;

        public AddReservationMessageRuleCommand(IGenericRepository<SeatQEntities, ReservationMessageRule> reservationRepository,
            IGenericRepository<SeatQEntities, UserInfo> userInfoRepository)
        {
            _reservationRepository = reservationRepository;
            _userInfoRepository = userInfoRepository;
        }

        public void Handle(AddReservationMessageRuleRequest request)
        {
  
            if(request.MessageType.Equals("Welcome"))
            {
                var exist = _reservationRepository.FindBy(x => x.BusinessDetailId == request.BusinessDetailId && x.MessageType.Equals("Welcome"));
                if (exist.Any())
                    throw new Exception("Welcome message already defined.");
            }

            _reservationRepository.Add(new ReservationMessageRule
            {
                BeforeAfter = request.BeforeAfter,
                InOut = request.InOut,
                Message = request.Message,
                MessageType = request.MessageType,
                BusinessDetailId = request.BusinessDetailId,
                Value = request.Value ?? 0,
                ValueType = request.ValueType
            });

            _reservationRepository.Save();
        }
    }
}
using common.api.Commands;
using common.dal;
using Hangfire;
using SeatQ.core.api.Infrastructure.NexmoMessaging;
using SeatQ.core.dal.Models;
using System;
using System.Linq;
using WebGrease.Css.Extensions;

namespace SeatQ.core.api.Commands.Reservation.TimeOutReservation
{
    public class TimeOutReservationCommand : ICommand<TimeOutReservationRequest>
    {
        private readonly IGenericRepository<SeatQEntities, dal.Models.Reservation> _reservationRepository;
        private readonly IGenericRepository<SeatQEntities, dal.Models.ReservationMessageRule> _reservationMessageRepository;
        private readonly IMessaging _messaging;

        public TimeOutReservationCommand(IGenericRepository<SeatQEntities, dal.Models.Reservation> reservationRepository,
            IGenericRepository<SeatQEntities, ReservationMessageRule> reservationMessageRepository,
            IMessaging messaging)
        {
            _reservationRepository = reservationRepository;
            _reservationMessageRepository = reservationMessageRepository;
            _messaging = messaging;
        }
        public void Handle(TimeOutReservationRequest request)
        {
            var reservataton = _reservationRepository.FindBy(x => x.Id == request.Id && x.BusinessDetailId == request.BusinessDetailId && x.TimeIn != null && x.TimeOut == null && x.IsCancelled != true).FirstOrDefault();
            if (reservataton != null)
            {
                reservataton.TimeOut = DateTime.UtcNow;
                _reservationRepository.Save();

                //Send Messaging if any
                SetTimeOutMessageSchedular(reservataton);
            }

        }

        private void SetTimeOutMessageSchedular(dal.Models.Reservation reservation)
        {
            var rule = _reservationMessageRepository.FindBy(x => x.BusinessDetailId == reservation.BusinessDetailId && x.InOut.Equals("TimeOut") && x.BeforeAfter.Equals("After"));
            if (rule.Any())
            {
                var virtualNo = reservation.BusinessDetail.VirtualNo;
                var guestNo = reservation.ReservationGuest.MobileNumber;

                rule.ForEach(x =>
                {
                    int minutes = (int)x.Value;
                    if (x.ValueType.Equals("Hours"))
                        minutes = (int)x.Value * 60;

                    var job = BackgroundJob.Schedule(() => _messaging.Send(virtualNo, guestNo, x.Message), TimeSpan.FromMinutes(minutes));
                });
            }
        }
    }
}
using System;
using System.Linq;
using Hangfire;
using iVeew.common.api.Commands;
using IVeew.common.dal;
using QTimes.api.Infrastructure.NexmoMessaging;
using QTimes.core.dal.Models;

namespace QTimes.api.Commands.Reservation.TimeInReservation
{
    public class TimeInReservationCommand : ICommand<TimeInReservationRequest>
    {
        private readonly IGenericRepository<QTimesContext, ReservationMessageRule> _reservationMessageRepository;
        private readonly IGenericRepository<QTimesContext, core.dal.Models.Reservation> _reservationRepository;
        private readonly IMessaging _messaging;

        public TimeInReservationCommand(IGenericRepository<QTimesContext, core.dal.Models.Reservation> reservationRepository,
            IGenericRepository<QTimesContext, ReservationMessageRule> reservationMessageRepository,
            IMessaging messaging)
        {
            _reservationRepository = reservationRepository;
            _reservationMessageRepository = reservationMessageRepository;
            _messaging = messaging;
        }
        public void Handle(TimeInReservationRequest request)
        {

            var reservataton = _reservationRepository.FindBy(x => x.Id == request.Id && x.BusinessDetailId == request.BusinessDetailId && x.TimeIn == null && x.IsCancelled != true).FirstOrDefault();
            if (reservataton != null)
            {
                reservataton.TimeIn = DateTime.UtcNow;
                _reservationRepository.Save();

                //Send Messaging if any
                SetTimeInMessageSchedular(reservataton);
            }

        }

        private void SetTimeInMessageSchedular(core.dal.Models.Reservation reservation)
        {
            var rule = _reservationMessageRepository.FindBy(x => x.BusinessDetailId == reservation.BusinessDetailId && x.InOut.Equals("TimeIn") && x.BeforeAfter.Equals("After"));

            var virtualNo = reservation.BusinessDetail.VirtualNo;
            var guestNo = reservation.ReservationGuest.MobileNumber;
            var outTime = reservation.UtcDateTimeTo.Value;//reservation.TimeIn.Value.AddMinutes(reservation.TimeTo.Value.TotalMinutes - reservation.TimeFrom.Value.TotalMinutes);

            rule.ToList().ForEach(x =>
            {
                int minutes = (int)x.Value;
                if (x.ValueType.Equals("Hours"))
                    minutes = (int)x.Value * 60;

                var job = BackgroundJob.Schedule(() => _messaging.Send(virtualNo, guestNo, x.Message), TimeSpan.FromMinutes(minutes));
            });

            rule = _reservationMessageRepository.FindBy(x => x.BusinessDetailId == reservation.BusinessDetailId && x.InOut.Equals("TimeOut") && x.BeforeAfter.Equals("Before"));
            rule.ToList().ForEach(x =>
            {
                int minutes = (int)x.Value;
                if (x.ValueType.Equals("Hours"))
                    minutes = (int)x.Value * 60;

                var messageTime = outTime.AddMinutes(-minutes);
                var fromMinutes = Convert.ToDateTime(messageTime).Subtract(DateTime.UtcNow).TotalMinutes;

                var job = BackgroundJob.Schedule(() => _messaging.Send(virtualNo, guestNo, x.Message), TimeSpan.FromMinutes(fromMinutes));
            });
        }
    }
}
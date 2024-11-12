using common.api.Commands;
using common.dal;
using SeatQ.core.dal.Models;
using System;
using System.Linq;
using Hangfire;
using SeatQ.core.api.Infrastructure.NexmoMessaging;
using WebGrease.Css.Extensions;

namespace SeatQ.core.api.Commands.Reservation.TimeInReservation
{
    public class TimeInReservationCommand : ICommand<TimeInReservationRequest>
    {
        private readonly IGenericRepository<SeatQEntities, dal.Models.ReservationMessageRule> _reservationMessageRepository;
        private readonly IGenericRepository<SeatQEntities, dal.Models.Reservation> _reservationRepository;
        private readonly IMessaging _messaging;

        public TimeInReservationCommand(IGenericRepository<SeatQEntities, dal.Models.Reservation> reservationRepository,
            IGenericRepository<SeatQEntities, ReservationMessageRule> reservationMessageRepository,
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

        private void SetTimeInMessageSchedular(dal.Models.Reservation reservation)
        {
            var rule = _reservationMessageRepository.FindBy(x => x.BusinessDetailId == reservation.BusinessDetailId && x.InOut.Equals("TimeIn") && x.BeforeAfter.Equals("After"));

            var virtualNo = reservation.BusinessDetail.VirtualNo;
            var guestNo = reservation.ReservationGuest.MobileNumber;
            var outTime = reservation.UtcDateTimeTo.Value;//reservation.TimeIn.Value.AddMinutes(reservation.TimeTo.Value.TotalMinutes - reservation.TimeFrom.Value.TotalMinutes);

            rule.ForEach(x =>
                {
                    int minutes = (int)x.Value;
                    if (x.ValueType.Equals("Hours"))
                        minutes = (int)x.Value * 60;

                    var job = BackgroundJob.Schedule(() => _messaging.Send(virtualNo, guestNo, x.Message), TimeSpan.FromMinutes(minutes));
                });

            rule = _reservationMessageRepository.FindBy(x => x.BusinessDetailId == reservation.BusinessDetailId && x.InOut.Equals("TimeOut") && x.BeforeAfter.Equals("Before"));
            rule.ForEach(x =>
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
using common.api.Commands;
using common.dal;
using Hangfire;
using SeatQ.core.api.Infrastructure.NexmoMessaging;
using SeatQ.core.api.Models.Pass;
using SeatQ.core.api.Services;
using SeatQ.core.dal.Models;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WebGrease.Css.Extensions;

namespace SeatQ.core.api.Commands.Reservation.AddReservation
{
    public class AddReservationCommand : IAsyncCommand<AddReservationRequest, string>
    {
        private readonly IGenericRepository<SeatQEntities, dal.Models.Reservation> _reservationRepository;
        private readonly IGenericRepository<SeatQEntities, ReservationGuest> _reservationGuestRepository;
        private readonly IGenericRepository<SeatQEntities, BusinessDetail> _reservationBusinessRepository;
        private readonly IGenericRepository<SeatQEntities, dal.Models.ReservationMessageRule> _reservationMessageRepository;
        private readonly IMessaging _messaging;
        private readonly IPasstrekClient _passtrekClient;

        public AddReservationCommand(IGenericRepository<SeatQEntities, dal.Models.Reservation> reservationRepository,
            IGenericRepository<SeatQEntities, ReservationGuest> reservationGuestRepository,
            IGenericRepository<SeatQEntities, BusinessDetail> reservationBusinessRepository,
            IGenericRepository<SeatQEntities, ReservationMessageRule> reservationMessageRepository,
            IMessaging messaging, IPasstrekClient passtrekClient)
        {
            _reservationRepository = reservationRepository;
            _reservationGuestRepository = reservationGuestRepository;
            _reservationBusinessRepository = reservationBusinessRepository;
            _reservationMessageRepository = reservationMessageRepository;
            _messaging = messaging;
            _passtrekClient = passtrekClient;
        }

        public async Task<string> Handle(AddReservationRequest request)
        {

            var business = _reservationBusinessRepository.FindBy(x => x.Id == request.BusinessDetailId).FirstOrDefault();
            if (business == null)
                throw new Exception("Reservation business not found.");

            ValidateLeftSpots(request, business);

            request.MobileNo = Regex.Replace(request.MobileNo, "[^0-9]", "");

            var guest = _reservationGuestRepository.FindBy(x => x.MobileNumber == request.MobileNo).FirstOrDefault();

            if (guest == null)
            {
                guest = new ReservationGuest
                {
                    Id = Guid.NewGuid(),
                    MobileNumber = request.MobileNo,
                    Name = request.Name,
                    BusinessDetailId = request.BusinessDetailId
                };
                _reservationGuestRepository.Add(guest);
            }
            else
                guest.Name = request.Name;

            _reservationGuestRepository.Save();

            var reservataton = new dal.Models.Reservation
            {
                GuestTypeId = request.GuestTypeId,
                Size = request.Size,
                ReservationGuestId = guest.Id,
                Date = request.Date,
                TimeFrom = request.TimeFrom,
                TimeTo = request.TimeTo,
                Comments = request.Comments,
                CreatedDateTime = DateTime.UtcNow,
                CreatedBy = request.CreatedBy,
                BusinessDetailId = request.BusinessDetailId,
                IsCancelled = false,
                UtcDateTimeFrom = DateTimeOffset.Parse(Convert.ToDateTime(request.Date.Value).Add(request.TimeFrom.Value).ToString() + " " + business.ReservationBusiness.TimezoneOffset).UtcDateTime,
                UtcDateTimeTo = DateTimeOffset.Parse(Convert.ToDateTime(request.Date.Value).Add(request.TimeTo.Value).ToString() + " " + business.ReservationBusiness.TimezoneOffset).UtcDateTime
            };

            _reservationRepository.Add(reservataton);
            _reservationRepository.Save();
            SendWelcomeMessage(request, guest);
            SetTimeInMessageSchedular(reservataton, guest, business);

            QTimesPassModel passModel = new QTimesPassModel()
            {
                TemplateId = (business.PassTemplateId ?? 0).ToString(),
                Name = guest.Name,
                MobileNumber = guest.MobileNumber,
                GroupSize = reservataton.Size,
                Date = reservataton.Date,
                TimeFrom = reservataton.TimeFrom,
                TimeTo = reservataton.TimeTo,
                Comments = reservataton.Comments
            };

            string passUrl = await _passtrekClient.GeneratePassUrlAsync(passModel);

            return passUrl;
        }

        private void SetTimeInMessageSchedular(dal.Models.Reservation reservation, ReservationGuest guest, BusinessDetail business)
        {
            var rule = _reservationMessageRepository.FindBy(x => x.BusinessDetailId == reservation.BusinessDetailId && x.InOut.Equals("TimeIn") && x.BeforeAfter.Equals("Before") && x.MessageType != "Welcome");

            var virtualNo = business.VirtualNo;
            var guestNo = guest.MobileNumber;
            var InTime = reservation.UtcDateTimeFrom.Value;//reservation.Date.Value.Add(reservation.TimeFrom.Value);

            rule.ForEach(x =>
            {
                int minutes = (int)x.Value;
                if (x.ValueType.Equals("Hours"))
                    minutes = (int)x.Value * 60;

                var messageTime = InTime.AddMinutes(-minutes);
                var fromMinutes = Convert.ToDateTime(messageTime).Subtract(DateTime.UtcNow).TotalMinutes;
                var job = BackgroundJob.Schedule(() => _messaging.Send(virtualNo, guestNo, x.Message), TimeSpan.FromMinutes(fromMinutes));
            });
        }

        private void ValidateLeftSpots(AddReservationRequest request, BusinessDetail business)
        {
            var sizeLimitPerGroup = business.LimitSizePerGroup ?? 0;
            var sizeLimitPerHour = business.LimitSizePerHour ?? 0;

            if (request.Size > sizeLimitPerGroup)
                throw new Exception("Size exceeded the group limit");

            var reservations = _reservationRepository.FindBy(x => x.BusinessDetailId == request.BusinessDetailId && x.TimeFrom == request.TimeFrom && x.TimeTo == request.TimeTo && x.Date == request.Date);
            if (reservations.Any())
            {
                var totalSeatsOccupied = reservations.Sum(x => x.Size);
                var remainingSeats = sizeLimitPerHour - totalSeatsOccupied;
                if (request.Size > remainingSeats)
                    throw new Exception("Size exceeded the limit");
            }
            else
            {
                if (request.Size > sizeLimitPerHour)
                    throw new Exception("Size exceeded the limit");
            }

        }

        private void SendWelcomeMessage(AddReservationRequest request, ReservationGuest guest)
        {
            var message = _reservationMessageRepository.FindBy(x => x.BusinessDetailId == request.BusinessDetailId && x.MessageType.Equals("Welcome"));
            if (message.Any())
            {
                var msg = message.FirstOrDefault();
                var virtualNo = msg.BusinessDetail.VirtualNo;
                var guestNo = guest.MobileNumber;
                if (!string.IsNullOrEmpty(msg.Message) && virtualNo != null && guestNo != null)
                {
                    _messaging.Send(virtualNo, guestNo, msg.Message);
                }
            }
        }
    }
}
using Hangfire;
using iVeew.common.api.Commands;
using IVeew.common.dal;
using QTimes.api.Exceptions;
using QTimes.api.Infrastructure.NexmoMessaging;
using QTimes.api.Models.Pass;
using QTimes.api.Services;
using QTimes.core.dal.Models;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace QTimes.api.Commands.Reservation.AddReservation
{
    public class AddReservationCommand : IAsyncCommand<AddReservationRequest, AddReservationResponse>
    {
        private readonly IGenericRepository<QTimesContext, core.dal.Models.Reservation> _reservationRepository;
        private readonly IGenericRepository<QTimesContext, ReservationGuest> _reservationGuestRepository;
        private readonly IGenericRepository<QTimesContext, BusinessDetail> _reservationBusinessRepository;
        private readonly IGenericRepository<QTimesContext, ReservationMessageRule> _reservationMessageRepository;
        private readonly IMessaging _messaging;
        private readonly IPasstrekClient _passtrekClient;

        public AddReservationCommand(IGenericRepository<QTimesContext, core.dal.Models.Reservation> reservationRepository,
            IGenericRepository<QTimesContext, ReservationGuest> reservationGuestRepository,
            IGenericRepository<QTimesContext, BusinessDetail> reservationBusinessRepository,
            IGenericRepository<QTimesContext, ReservationMessageRule> reservationMessageRepository,
            IMessaging messaging, IPasstrekClient passtrekClient)
        {
            _reservationRepository = reservationRepository;
            _reservationGuestRepository = reservationGuestRepository;
            _reservationBusinessRepository = reservationBusinessRepository;
            _reservationMessageRepository = reservationMessageRepository;
            _messaging = messaging;
            _passtrekClient = passtrekClient;
        }

        public async Task<AddReservationResponse> Handle(AddReservationRequest request)
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
                    BusinessDetailId = request.BusinessDetailId,
                    Address = request.Address,
                    City = request.City,
                    Dob = request.Dob,
                    FamilyName = request.FamilyName,
                    FirstName = request.FirstName,
                    State = request.State,
                    Zip = request.Zip,
                    FacePhotoImageBase64 = request.FacePhotoImageBase64,
                    Email = request.Email
                };
                _reservationGuestRepository.Add(guest);
            }
            else
                guest.Name = request.Name;

            _reservationGuestRepository.Save();

            var reservataton = new core.dal.Models.Reservation
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
            QTimesPassModel passModel = new QTimesPassModel()
            {
                TemplateId = (business.PassTemplateId ?? 0).ToString(),
                Name = guest.Name,
                MobileNumber = guest.MobileNumber,
                GroupSize = reservataton.Size,
                Date = reservataton.Date,
                TimeFrom = reservataton.TimeFrom,
                TimeTo = reservataton.TimeTo,
                Comments = reservataton.Comments,
                Address = guest.Address,
                City = guest.City,
                DOB = guest.Dob.ToString(),
                FamiliName = guest.FamilyName,
                FirstName = guest.FirstName,
                State = guest.State,
                Zip = guest.Zip,
                FacePhotoImageBase64 = guest.FacePhotoImageBase64
            };
            string passUrl = await _passtrekClient.GeneratePassUrlAsync(passModel);
            reservataton.PassUrl = passUrl;
            _reservationRepository.Add(reservataton);
            _reservationRepository.Save();
            SendWelcomeMessage(request, guest);
            SetTimeInMessageSchedular(reservataton, guest, business);
            SendPassLink(reservataton, guest, business, passUrl);
            return new AddReservationResponse()
            {
                ReservationId = reservataton.Id,
                PassUrl = passUrl
            };
        }

        private void SendPassLink(core.dal.Models.Reservation reservataton, ReservationGuest guest, BusinessDetail business, string url)
        {
            var virtualNo = business.VirtualNo;
            var guestNo = guest.MobileNumber;
            if (!string.IsNullOrWhiteSpace(virtualNo) && !string.IsNullOrWhiteSpace(guestNo))
            {
                var job = BackgroundJob.Enqueue(() => _messaging.Send(virtualNo, guestNo, $"Add reservation pass to the Wallet {url}"));
            }
        }

        private void SetTimeInMessageSchedular(core.dal.Models.Reservation reservation, ReservationGuest guest, BusinessDetail business)
        {
            var rule = _reservationMessageRepository.FindBy(x => x.BusinessDetailId == reservation.BusinessDetailId && x.InOut.Equals("TimeIn") && x.BeforeAfter.Equals("Before") && x.MessageType != "Welcome");

            var virtualNo = business.VirtualNo;
            var guestNo = guest.MobileNumber;
            var InTime = reservation.UtcDateTimeFrom.Value;//reservation.Date.Value.Add(reservation.TimeFrom.Value);

            rule.ToList().ForEach(x =>
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
                throw new InvalidReservationDataException("Size exceeded the group limit");

            var reservations = _reservationRepository.FindBy(x => x.BusinessDetailId == request.BusinessDetailId && x.TimeFrom == request.TimeFrom && x.TimeTo == request.TimeTo && x.Date == request.Date);
            if (reservations.Any())
            {
                var totalSeatsOccupied = reservations.Sum(x => x.Size);
                var remainingSeats = sizeLimitPerHour - totalSeatsOccupied;
                if (request.Size > remainingSeats)
                    throw new InvalidReservationDataException("Size exceeded the limit");
            }
            else
            {
                if (request.Size > sizeLimitPerHour)
                    throw new InvalidReservationDataException("Size exceeded the limit");
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
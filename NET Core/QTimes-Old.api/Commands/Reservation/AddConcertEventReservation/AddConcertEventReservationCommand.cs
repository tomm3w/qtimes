using Hangfire;
using iVeew.common.api.Commands;
using IVeew.common.dal;
using QTimes.api.Infrastructure.NexmoMessaging;
using QTimes.api.Models.Pass;
using QTimes.api.Services;
using QTimes.core.dal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace QTimes.api.Commands.Reservation.AddConcertEventReservation
{
    public class AddConcertEventReservationCommand : IAsyncCommand<AddConcertEventReservationRequest, AddConcertEventReservationResponse>
    {
        private readonly IGenericRepository<QTimesContext, ConcertEvent> _concertEventRepository;
        private readonly IGenericRepository<QTimesContext, ConcertEventSeatLock> _concertEventSeatLockRepository;
        private readonly IGenericRepository<QTimesContext, ConcertEventReservation> _reservationRepository;
        private readonly IGenericRepository<QTimesContext, ConcertEventMessageRule> _reservationMessageRepository;
        private readonly IMessaging _messaging;
        private readonly IPasstrekClient _passtrekClient;

        public AddConcertEventReservationCommand(IGenericRepository<QTimesContext, ConcertEventReservation> reservationRepository,
            IGenericRepository<QTimesContext, ConcertEventMessageRule> reservationMessageRepository,
            IGenericRepository<QTimesContext, ConcertEvent> concertEventRepository,
            IGenericRepository<QTimesContext, ConcertEventSeatLock> concertSeatLockRepository,
            IMessaging messaging, IPasstrekClient passtrekClient)
        {
            _reservationRepository = reservationRepository;
            _reservationMessageRepository = reservationMessageRepository;
            _messaging = messaging;
            _passtrekClient = passtrekClient;
            _concertEventRepository = concertEventRepository;
            _concertEventSeatLockRepository = concertSeatLockRepository;
        }

        public async Task<AddConcertEventReservationResponse> Handle(AddConcertEventReservationRequest request)
        {
            var concert = _concertEventRepository.FindBy(x => x.Id == request.ConcertEventId).FirstOrDefault();
            if (concert == null)
                throw new Exception("Concert event not found.");

            request.MainGuestMobileNo = Regex.Replace(request.MainGuestMobileNo, "[^0-9]", "");
            ValidateSeats(request);

            var reservation = new ConcertEventReservation
            {
                Seatings = request.Seatings,
                Size = request.Size,
                GuestTypeId = request.GuestTypeId,
                ConcertEventId = request.ConcertEventId
            };

            var mainGuest = new ConcertEventGuest
            {
                Name = request.MainGuestName,
                MobileNo = request.MainGuestMobileNo,
                IsMainGuest = true,
                SeatNo = $"{request.MainGuestSeatName}-{request.MainGuestSeatNo}",
                Email = request.MainGuestEmail,
                ConcertEventReservation = reservation,
                Address = request.Address,
                City = request.City,
                Dob = request.Dob,
                FamilyName = request.FamilyName,
                FirstName = request.FirstName,
                State = request.State,
                Zip = request.Zip,
                FacePhotoImageBase64 = request.FacePhotoImageBase64
            };

            reservation.ConcertEventGuest.Add(mainGuest);

            if (request.Guests != null)
            {
                foreach (var guest in request.Guests)
                {
                    var participant = new ConcertEventGuest
                    {
                        Name = guest.Name,
                        MobileNo = Regex.Replace(guest.MobileNo, "[^0-9]", ""),
                        IsMainGuest = false,
                        SeatNo = $"{guest.SeatName}-{guest.SeatNo}",
                        ConcertEventReservation = reservation
                    };
                    reservation.ConcertEventGuest.Add(participant);
                }
            }

            QTimesPassModel passModel = new QTimesPassModel()
            {
                TemplateId = (concert.PassTemplateId ?? 0).ToString(),
                Name = request.MainGuestName,
                MobileNumber = request.MainGuestMobileNo,
                GroupSize = request.Size,
                Date = concert.Date,
                TimeFrom = concert.TimeFrom,
                TimeTo = concert.TimeTo,
                Address = request.Address,
                City = request.City,
                DOB = request.Dob.ToString(),
                FamiliName = request.FamilyName,
                FirstName = request.FirstName,
                State = request.State,
                Zip = request.Zip,
                FacePhotoImageBase64 = request.FacePhotoImageBase64
            };

            string passUrl = await _passtrekClient.GeneratePassUrlAsync(passModel);
            reservation.PassUrl = passUrl;

            _reservationRepository.Add(reservation);
            _reservationRepository.Save();

            UnlockSeats(request);
            SendWelcomeMessage(request);
            SetTimeInMessageSchedular(request);
            SendPassLink(reservation, mainGuest, concert, passUrl);
            return new AddConcertEventReservationResponse()
            {
                ConcertEventReservationId = reservation.Id,
                PassUrl = passUrl
            };
        }

        private void SendPassLink(ConcertEventReservation reservataton, ConcertEventGuest guest, ConcertEvent concert, string url)
        {
            var virtualNo = concert.VirtualNo;
            var guestNo = guest.MobileNo;
            if (!string.IsNullOrWhiteSpace(virtualNo) && !string.IsNullOrWhiteSpace(guestNo))
            {
                var job = BackgroundJob.Enqueue(() => _messaging.Send(virtualNo, guestNo, $"Add reservation pass to the Wallet {url}"));
            }
        }

        private async void SendWelcomeMessage(AddConcertEventReservationRequest request)
        {
            var message = _reservationMessageRepository.FindBy(x => x.ConcertEventId == request.ConcertEventId && x.MessageType.Equals("Welcome"));
            if (message.Any())
            {
                var msg = message.FirstOrDefault();
                var virtualNo = msg.ConcertEvent.VirtualNo;
                var guestNo = request.MainGuestMobileNo;
                var allowCheckIn = msg.ConcertEvent.AllowCheckInTime;

                if (!string.IsNullOrEmpty(msg.Message) && virtualNo != null && guestNo != null)
                {
                    string msgText = msg.Message;
                    if (allowCheckIn == true)
                    {
                        var checkInTime = msg.ConcertEvent.ConcertEventSeating.Where(x => x.Name == request.MainGuestSeatName).FirstOrDefault();
                        if (checkInTime != null)
                        {
                            if (checkInTime.CheckInTime != null)
                                msgText = msgText + " Chek-in time:" + checkInTime.CheckInTime;
                        }
                    }

                    //string passUrl = await GetPassUrl(request);
                    //passUrl = !string.IsNullOrEmpty(passUrl) ? $" Pls click this link for your pass. {passUrl}" : "";
                    //msgText = msgText + passUrl;

                    _messaging.Send(virtualNo, guestNo, msgText);
                }
            }
        }

        private void SetTimeInMessageSchedular(AddConcertEventReservationRequest request)
        {
            var rule = _reservationMessageRepository.FindBy(x => x.ConcertEventId == request.ConcertEventId && x.InOut.Equals("TimeIn") && x.BeforeAfter.Equals("Before") && x.MessageType != "Welcome");
            if (rule.Any())
            {
                var msg = rule.FirstOrDefault();
                var virtualNo = msg.ConcertEvent.VirtualNo;
                var guestNo = request.MainGuestMobileNo;
                var InTime = msg.ConcertEvent.Date.Value.Add(msg.ConcertEvent.TimeFrom.Value);

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
        }

        private void ValidateSeats(AddConcertEventReservationRequest request)
        {
            var occupiedSeats = new List<string>();
            string seatNo = $"{request.MainGuestSeatName}-{request.MainGuestSeatNo}";
            var mainGuestSeatsTaken = _reservationRepository.FindBy(x => x.ConcertEventId == request.ConcertEventId && x.ConcertEventGuest.Any(a => a.SeatNo == seatNo));
            if (mainGuestSeatsTaken.Count() > 0)
                occupiedSeats.Add($"{request.MainGuestSeatName}-{request.MainGuestSeatNo}");

            if (request.Guests != null)
            {
                foreach (var guest in request.Guests)
                {
                    string seatNum = $"{guest.SeatName}-{guest.SeatNo}";
                    var seatsTaken = _reservationRepository.FindBy(x => x.ConcertEventId == request.ConcertEventId && x.ConcertEventGuest.Any(a => a.SeatNo == seatNum));
                    if (seatsTaken.Count() > 0)
                        occupiedSeats.Add($"{guest.SeatName}-{guest.SeatNo}");
                }
            }

            if (occupiedSeats.Count > 0)
                throw new Exception("Following seats already taken: " + string.Join(", ", occupiedSeats.Select(x => x)));

        }

        private void UnlockSeats(AddConcertEventReservationRequest request)
        {
            var occupiedSeats = new List<string>();
            string seatNo = $"{request.MainGuestSeatName}-{request.MainGuestSeatNo}";
            var mainGuestSeatsTaken = _concertEventSeatLockRepository.FindBy(x => x.ConcertEventId == request.ConcertEventId && x.SeatNo == seatNo).FirstOrDefault();
            if (mainGuestSeatsTaken != null)
                _concertEventSeatLockRepository.Delete(mainGuestSeatsTaken);

            if (request.Guests != null)
            {
                foreach (var guest in request.Guests)
                {
                    string seatNum = $"{guest.SeatName}-{guest.SeatNo}";
                    var seatsTaken = _concertEventSeatLockRepository.FindBy(x => x.ConcertEventId == request.ConcertEventId && x.SeatNo == seatNum).FirstOrDefault();
                    if (seatsTaken != null)
                        _concertEventSeatLockRepository.Delete(seatsTaken);
                }
            }

            _concertEventSeatLockRepository.Save();
        }

        private async Task<string> GetPassUrl(AddConcertEventReservationRequest request)
        {
            string passUrl = "";
            var concert = _concertEventRepository.FindBy(x => x.Id == request.ConcertEventId).FirstOrDefault();
            if (concert != null && concert.PassTemplateId != null)
            {
                QTimesPassModel passModel = new QTimesPassModel()
                {
                    TemplateId = concert.PassTemplateId.ToString(),
                    Name = request.MainGuestName,
                    MobileNumber = request.MainGuestMobileNo,
                    GroupSize = request.Size,
                    Date = concert.Date,
                    TimeFrom = concert.TimeFrom,
                    TimeTo = concert.TimeTo
                };

                passUrl = await _passtrekClient.GeneratePassUrlAsync(passModel);
            }

            return passUrl;
        }
    }
}
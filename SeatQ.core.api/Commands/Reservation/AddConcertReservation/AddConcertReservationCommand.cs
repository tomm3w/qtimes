using common.api.Commands;
using common.dal;
using Hangfire;
using SeatQ.core.api.Infrastructure.NexmoMessaging;
using SeatQ.core.api.Models.Pass;
using SeatQ.core.api.Services;
using SeatQ.core.dal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebGrease.Css.Extensions;

namespace SeatQ.core.api.Commands.Reservation.AddConcertReservation
{
    public class AddConcertReservationCommand : ICommand<AddConcertReservationRequest>
    {
        private readonly IGenericRepository<SeatQEntities, Concert> _concertRepository;
        private readonly IGenericRepository<SeatQEntities, ConcertSeatLock> _concertSeatLockRepository;
        private readonly IGenericRepository<SeatQEntities, ConcertReservation> _reservationRepository;
        private readonly IGenericRepository<SeatQEntities, dal.Models.ConcertMessageRule> _reservationMessageRepository;
        private readonly IMessaging _messaging;
        private readonly IPasstrekClient _passtrekClient;

        public AddConcertReservationCommand(IGenericRepository<SeatQEntities, ConcertReservation> reservationRepository,
            IGenericRepository<SeatQEntities, ConcertMessageRule> reservationMessageRepository,
            IGenericRepository<SeatQEntities, Concert> concertRepository,
            IGenericRepository<SeatQEntities, ConcertSeatLock> concertSeatLockRepository,
            IMessaging messaging, IPasstrekClient passtrekClient)
        {
            _reservationRepository = reservationRepository;
            _reservationMessageRepository = reservationMessageRepository;
            _messaging = messaging;
            _passtrekClient = passtrekClient;
            _concertRepository = concertRepository;
            _concertSeatLockRepository = concertSeatLockRepository;
        }
        public void Handle(AddConcertReservationRequest request)
        {

            ValidateSeats(request);

            var reservation = new ConcertReservation
            {
                ReservationDate = request.ReservationDate,
                Seatings = request.Seatings,
                Size = request.Size,
                ConcertId = request.ConcertId,
                GuestTypeId = request.GuestTypeId
            };

            reservation.ConcertGuests.Add(new ConcertGuest
            {
                Name = request.MainGuestName,
                MobileNo = request.MainGuestMobileNo,
                IsMainGuest = true,
                SeatNo = $"{request.MainGuestSeatName}-{request.MainGuestSeatNo}",
                Email = request.MainGuestEmail
            });

            if (request.Guests != null)
            {
                foreach (var guest in request.Guests)
                {
                    var participant = new ConcertGuest
                    {
                        Name = guest.Name,
                        MobileNo = guest.MobileNo,
                        IsMainGuest = false,
                        SeatNo = $"{guest.SeatName}-{guest.SeatNo}"
                    };
                    reservation.ConcertGuests.Add(participant);
                }
            }

            _reservationRepository.Add(reservation);
            _reservationRepository.Save();

            UnlockSeats(request);
            SendWelcomeMessage(request);
            SetTimeInMessageSchedular(request);
        }

        private async void SendWelcomeMessage(AddConcertReservationRequest request)
        {
            var message = _reservationMessageRepository.FindBy(x => x.ConcertId == request.ConcertId && x.MessageType.Equals("Welcome"));
            if (message.Any())
            {
                var msg = message.FirstOrDefault();
                var virtualNo = msg.Concert.VirtualNo;
                var guestNo = request.MainGuestMobileNo;
                var allowCheckIn = msg.Concert.AllowCheckInTime;

                if (!string.IsNullOrEmpty(msg.Message) && virtualNo != null && guestNo != null)
                {
                    string msgText = msg.Message;
                    if (allowCheckIn == true)
                    {
                        var checkInTime = msg.Concert.ConcertSeatings.Where(x => x.Name == request.MainGuestSeatName).FirstOrDefault();
                        if (checkInTime != null)
                        {
                            if (checkInTime.CheckInTime != null)
                                msgText = msgText + " Chek-in time:" + checkInTime.CheckInTime;
                        }
                    }

                    string passUrl = await GetPassUrl(request);
                    passUrl = !string.IsNullOrEmpty(passUrl) ? $" Pls click this link for your pass. {passUrl}" : "";

                    msgText = msgText + passUrl;
                    _messaging.Send(virtualNo, guestNo, msgText);
                }
            }
        }

        private async Task<string> GetPassUrl(AddConcertReservationRequest request)
        {
            string passUrl = "";
            var concert = _concertRepository.FindBy(x => x.Id == request.ConcertId).FirstOrDefault();
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

        private void SetTimeInMessageSchedular(AddConcertReservationRequest request)
        {
            var rule = _reservationMessageRepository.FindBy(x => x.ConcertId == request.ConcertId && x.InOut.Equals("TimeIn") && x.BeforeAfter.Equals("Before") && x.MessageType != "Welcome");
            if (rule.Any())
            {
                var msg = rule.FirstOrDefault();
                var virtualNo = msg.Concert.VirtualNo;
                var guestNo = request.MainGuestMobileNo;
                var InTime = msg.Concert.Date.Value.Add(msg.Concert.TimeFrom.Value);

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
        }

        private void ValidateSeats(AddConcertReservationRequest request)
        {
            var occupiedSeats = new List<string>();
            string seatNo = $"{request.MainGuestSeatName}-{request.MainGuestSeatNo}";
            var mainGuestSeatsTaken = _reservationRepository.FindBy(x => x.ConcertId == request.ConcertId && x.ConcertGuests.Any(a => a.SeatNo == seatNo));
            if (mainGuestSeatsTaken.Count() > 0)
                occupiedSeats.Add($"{request.MainGuestSeatName}-{request.MainGuestSeatNo}");

            if (request.Guests != null)
            {
                foreach (var guest in request.Guests)
                {
                    string seatNum = $"{guest.SeatName}-{guest.SeatNo}";
                    var seatsTaken = _reservationRepository.FindBy(x => x.ConcertId == request.ConcertId && x.ConcertGuests.Any(a => a.SeatNo == seatNum));
                    if (seatsTaken.Count() > 0)
                        occupiedSeats.Add($"{guest.SeatName}-{guest.SeatNo}");
                }
            }

            if (occupiedSeats.Count > 0)
                throw new Exception("Following seats already taken: " + string.Join(", ", occupiedSeats.Select(x => x)));

        }

        private void UnlockSeats(AddConcertReservationRequest request)
        {
            var occupiedSeats = new List<string>();
            string seatNo = $"{request.MainGuestSeatName}-{request.MainGuestSeatNo}";
            var mainGuestSeatsTaken = _concertSeatLockRepository.FindBy(x => x.ConcertId == request.ConcertId && x.SeatNo == seatNo).FirstOrDefault();
            if (mainGuestSeatsTaken != null)
                _concertSeatLockRepository.Delete(mainGuestSeatsTaken);

            if (request.Guests != null)
            {
                foreach (var guest in request.Guests)
                {
                    string seatNum = $"{guest.SeatName}-{guest.SeatNo}";
                    var seatsTaken = _concertSeatLockRepository.FindBy(x => x.ConcertId == request.ConcertId && x.SeatNo == seatNum).FirstOrDefault();
                    if (seatsTaken != null)
                        _concertSeatLockRepository.Delete(seatsTaken);
                }
            }

            _concertSeatLockRepository.Save();
        }
    }
}
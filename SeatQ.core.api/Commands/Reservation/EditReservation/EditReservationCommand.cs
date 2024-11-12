using common.api.Commands;
using common.dal;
using SeatQ.core.dal.Models;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace SeatQ.core.api.Commands.Reservation.EditReservation
{
    public class EditReservationCommand : ICommand<EditReservationRequest>
    {
        private readonly IGenericRepository<SeatQEntities, dal.Models.Reservation> _reservationRepository;
        private readonly IGenericRepository<SeatQEntities, ReservationGuest> _reservationGuestRepository;
        private readonly IGenericRepository<SeatQEntities, BusinessDetail> _reservationBusinessRepository;

        public EditReservationCommand(IGenericRepository<SeatQEntities, dal.Models.Reservation> reservationRepository,
            IGenericRepository<SeatQEntities, ReservationGuest> reservationGuestRepository,
            IGenericRepository<SeatQEntities, BusinessDetail> reservationBusinessRepository)
        {
            _reservationRepository = reservationRepository;
            _reservationGuestRepository = reservationGuestRepository;
            _reservationBusinessRepository = reservationBusinessRepository;
        }

        public void Handle(EditReservationRequest request)
        {

            var business = _reservationBusinessRepository.FindBy(x => x.Id == request.BusinessDetailId).FirstOrDefault();
            if (business == null)
                throw new Exception("Reservation business not found.");

            ValidateLeftSpots(request, business);

            request.MobileNo = Regex.Replace(request.MobileNo, "[^0-9]", "");
            var guest = _reservationGuestRepository.FindBy(x => x.Id == request.ReservationGuestId).FirstOrDefault();

            if (guest != null)
            {
                guest.Name = request.Name;
                guest.MobileNumber = request.MobileNo;
                _reservationGuestRepository.Save();
            }
            else
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

            var reservataton = _reservationRepository.FindBy(x => x.Id == request.Id && x.BusinessDetailId == request.BusinessDetailId).FirstOrDefault();
            if (reservataton != null)
            {
                reservataton.GuestTypeId = request.GuestTypeId;
                reservataton.Size = request.Size;
                reservataton.ReservationGuestId = guest.Id;
                reservataton.Date = request.Date;
                reservataton.TimeFrom = request.TimeFrom;
                reservataton.TimeTo = request.TimeTo;
                reservataton.Comments = request.Comments;
                reservataton.UtcDateTimeFrom = DateTimeOffset.Parse(Convert.ToDateTime(request.Date.Value).Add(request.TimeFrom.Value).ToString() + " " + business.ReservationBusiness.TimezoneOffset).UtcDateTime;
                reservataton.UtcDateTimeTo = DateTimeOffset.Parse(Convert.ToDateTime(request.Date.Value).Add(request.TimeTo.Value).ToString() + " " + business.ReservationBusiness.TimezoneOffset).UtcDateTime;
                _reservationRepository.Save();
            }

        }


        private void ValidateLeftSpots(EditReservationRequest request, BusinessDetail business)
        {
            var sizeLimitPerGroup = business.LimitSizePerGroup ?? 0;
            var sizeLimitPerHour = business.LimitSizePerHour ?? 0;

            if (request.Size > sizeLimitPerGroup)
                throw new Exception("Size exceeded the group limit");

            var reservations = _reservationRepository.FindBy(x => x.BusinessDetailId == request.BusinessDetailId && x.TimeFrom == request.TimeFrom
                            && x.TimeTo == request.TimeTo && x.Id != request.Id && x.Date == request.Date);
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
    }
}
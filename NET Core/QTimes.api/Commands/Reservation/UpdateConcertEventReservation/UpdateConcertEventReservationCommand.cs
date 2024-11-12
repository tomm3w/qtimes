using iVeew.common.api.Commands;
using IVeew.common.dal;
using QTimes.api.Exceptions;
using QTimes.core.dal.Models;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;

namespace QTimes.api.Commands.Reservation.UpdateConcertEventReservation
{
    public class UpdateConcertEventReservationCommand : ICommand<UpdateConcertEventReservationRequest>
    {
        private readonly IGenericRepository<QTimesContext, ConcertEventReservation> _reservationRepository;
        private readonly IGenericRepository<QTimesContext, ConcertEventGuest> _guestRepository;
        public UpdateConcertEventReservationCommand(IGenericRepository<QTimesContext, ConcertEventReservation> reservationRepository,
            IGenericRepository<QTimesContext, ConcertEventGuest> guestRepository)
        {
            _reservationRepository = reservationRepository;
            _guestRepository = guestRepository;
        }

        public void Handle(UpdateConcertEventReservationRequest request)
        {
            if (request.Guests != null)
            {
                var anyDuplicate = request.Guests.GroupBy(x => $"{x.SeatName}-{x.SeatNo}").Any(g => g.Count() > 1);
                if (anyDuplicate)
                    throw new ValidationException("Cannot add duplicate seat.");
            }

            var reservation = _reservationRepository.FindBy(x => x.Id == request.Id).FirstOrDefault();
            if (reservation == null)
                throw new NotFoundException();

            reservation.Size = request.Size;
            reservation.GuestTypeId = request.GuestTypeId;

            var mainGuest = reservation.ConcertEventGuest.Where(x => x.IsMainGuest == true).FirstOrDefault();
            if (mainGuest != null)
            {
                mainGuest.Name = request.MainGuestName;
                mainGuest.MobileNo = Regex.Replace(request.MainGuestMobileNo, "[^0-9]", "");
                mainGuest.Email = request.MainGuestEmail;
                mainGuest.SeatNo = $"{request.MainGuestSeatName}-{request.MainGuestSeatNo}";
            }

            var guests = _guestRepository.FindBy(x => x.ConcertEventReservationId == request.Id && x.IsMainGuest != true);
            if (guests.Any())
            {
                foreach (var item in guests)
                {
                    _guestRepository.Delete(item);
                }
                _guestRepository.Save();
            }

            if (request.Guests != null)
            {
                foreach (var guest in request.Guests)
                {
                    var participant = new ConcertEventGuest
                    {
                        Name = guest.Name,
                        MobileNo = Regex.Replace(guest.MobileNo, "[^0-9]", ""),
                        IsMainGuest = false,
                        SeatNo = $"{guest.SeatName}-{guest.SeatNo}"
                    };
                    reservation.ConcertEventGuest.Add(participant);
                }
            }

            _reservationRepository.Edit(reservation);
            _reservationRepository.Save();
        }
    }
}
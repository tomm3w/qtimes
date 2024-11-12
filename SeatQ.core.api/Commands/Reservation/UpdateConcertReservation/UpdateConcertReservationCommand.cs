using common.api.Commands;
using common.dal;
using SeatQ.core.api.Exceptions;
using SeatQ.core.dal.Models;
using System.Linq;
using WebGrease.Css.Extensions;

namespace SeatQ.core.api.Commands.Reservation.UpdateConcertReservation
{
    public class UpdateConcertReservationCommand : ICommand<UpdateConcertReservationRequest>
    {
        private readonly IGenericRepository<SeatQEntities, ConcertReservation> _reservationRepository;
        private readonly IGenericRepository<SeatQEntities, ConcertGuest> _guestRepository;
        public UpdateConcertReservationCommand(IGenericRepository<SeatQEntities, ConcertReservation> reservationRepository,
            IGenericRepository<SeatQEntities, ConcertGuest> guestRepository)
        {
            _reservationRepository = reservationRepository;
            _guestRepository = guestRepository;
        }

        public void Handle(UpdateConcertReservationRequest request)
        {
            var reservation = _reservationRepository.FindBy(x => x.Id == request.Id).FirstOrDefault();
            if (reservation == null)
                throw new NotFoundException();

            reservation.Size = request.Size;
            reservation.GuestTypeId = request.GuestTypeId;

            var mainGuest = reservation.ConcertGuests.Where(x => x.IsMainGuest == true).FirstOrDefault();
            if(mainGuest!=null)
            {
                mainGuest.Name = request.MainGuestName;
                mainGuest.MobileNo = request.MainGuestMobileNo;
                mainGuest.Email = request.MainGuestEmail;
                mainGuest.SeatNo = $"{request.MainGuestSeatName}-{request.MainGuestSeatNo}";
            }

            var guests = _guestRepository.FindBy(x => x.ConcertReservationId == request.Id && x.IsMainGuest != true);
            if (guests.Any())
            {
                guests.ForEach(item =>
                    _guestRepository.Delete(item)
                );
                _guestRepository.Save();
            }

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
        }
    }
}
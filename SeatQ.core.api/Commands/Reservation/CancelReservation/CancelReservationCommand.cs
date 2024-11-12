using common.api.Commands;
using common.dal;
using SeatQ.core.dal.Models;
using System;
using System.Linq;

namespace SeatQ.core.api.Commands.Reservation.CancelReservation
{
    public class CancelReservationCommand : ICommand<CancelReservationRequest>
    {
        private readonly IGenericRepository<SeatQEntities, dal.Models.Reservation> _reservationRepository;

        public CancelReservationCommand(IGenericRepository<SeatQEntities, dal.Models.Reservation> reservationRepository)
        {
            _reservationRepository = reservationRepository;
        }
        public void Handle(CancelReservationRequest request)
        {

            var reservataton = _reservationRepository.FindBy(x => x.Id == request.Id && x.BusinessDetailId == request.BusinessDetailId && x.IsCancelled != true).FirstOrDefault();
            if (reservataton != null)
            {
                reservataton.IsCancelled = true;
                reservataton.CancelledDateTime = DateTime.UtcNow;
                _reservationRepository.Save();
            }

        }
    }
}
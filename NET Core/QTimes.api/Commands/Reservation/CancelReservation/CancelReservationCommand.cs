using iVeew.common.api.Commands;
using IVeew.common.dal;
using QTimes.core.dal.Models;
using System;
using System.Linq;

namespace QTimes.api.Commands.Reservation.CancelReservation
{
    public class CancelReservationCommand : ICommand<CancelReservationRequest>
    {
        private readonly IGenericRepository<QTimesContext, QTimes.core.dal.Models.Reservation> _reservationRepository;

        public CancelReservationCommand(IGenericRepository<QTimesContext, QTimes.core.dal.Models.Reservation> reservationRepository)
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
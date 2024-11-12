using iVeew.common.api.Commands;
using IVeew.common.dal;
using QTimes.core.dal.Models;
using System;
using System.Linq;

namespace QTimes.api.Commands.Reservation.Preference
{
    public class PreferenceCommand : ICommand<PreferenceRequest>
    {
        private readonly IGenericRepository<QTimesContext, UserInfo> _userInfoRepository;
        private readonly IGenericRepository<QTimesContext, BusinessDetail> _reservationRepository;

        public PreferenceCommand(IGenericRepository<QTimesContext, UserInfo> userInfoRepository,
            IGenericRepository<QTimesContext, BusinessDetail> reservationRepository)
        {
            _userInfoRepository = userInfoRepository;
            _reservationRepository = reservationRepository;
        }

        public void Handle(PreferenceRequest request)
        {

            var business = _reservationRepository.FindBy(x => x.Id == request.Id).FirstOrDefault();

            if (business == null)
                throw new Exception("Reservation business not registered.");

            business.OpenDayFrom = request.OpenDayFrom;
            business.OpenDayTo = request.OpenDayTo;
            business.OpenHourFrom = request.OpenHourFrom;
            business.OpenHourTo = request.OpenHourTo;
            business.IsLimitSizePerHour = request.IsLimitSizePerHour;
            business.LimitSizePerHour = request.LimitSizePerHour;
            business.IsLimitTimePerReservation = request.IsLimitTimePerReservation;
            business.MinLimitTimePerReservation = request.MinLimitTimePerReservation;
            business.MaxLimitTimePerReservation = request.MaxLimitTimePerReservation;
            business.IsLimitSizePerGroup = request.IsLimitSizePerGroup;
            business.LimitSizePerGroup = request.LimitSizePerGroup;
            business.IsChargeEnabled = request.IsChargeEnabled;
            business.IsDlscanEnabled = request.IsDLScanEnabled;
            business.IsSeatmapEnabled = request.IsSeatmapEnabled;
            business.NoOfSeatRows = request.NoOfSeatRows;
            business.NoOfSeatColumns = request.NoOfSeatColumns;

            _reservationRepository.Save();
        }
    }
}
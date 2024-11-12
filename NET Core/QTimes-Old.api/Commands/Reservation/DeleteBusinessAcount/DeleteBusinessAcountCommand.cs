using iVeew.common.api.Commands;
using IVeew.common.dal;
using QTimes.core.dal.Models;
using System;
using System.Linq;

namespace QTimes.api.Commands.Reservation.DeleteBusinessAcount
{
    public class DeleteBusinessAcountCommand : ICommand<DeleteBusinessAcountRequest>
    {
        private readonly IGenericRepository<QTimesContext, UserInfo> _userInfoRepository;
        private readonly IGenericRepository<QTimesContext, BusinessDetail> _businessRepository;
        private readonly IGenericRepository<QTimesContext, Concert> _concertRepository;

        public DeleteBusinessAcountCommand(IGenericRepository<QTimesContext, UserInfo> userInfoRepository,
           IGenericRepository<QTimesContext, BusinessDetail> businessRepository, IGenericRepository<QTimesContext, Concert> concertRepository)
        {
            _userInfoRepository = userInfoRepository;
            _businessRepository = businessRepository;
            _concertRepository = concertRepository;
        }

        public void Handle(DeleteBusinessAcountRequest request)
        {
            var user = _userInfoRepository.FindBy(x => x.UserId == request.UserId).FirstOrDefault();
            if (user.ReservationBusinessId == null)
                throw new Exception("Reservation business not registered.");

            if (request.BusinessType == Models.Enums.BusinessTypeEnum.Generic)
            {
                var business = _businessRepository.FindBy(x => x.Id == request.Id && x.ReservationBusinessId == user.ReservationBusinessId).FirstOrDefault();
                if (business != null)
                {
                    business.DeletedDate = DateTime.UtcNow;
                    _businessRepository.Save();
                }
            }
            else
            {
                var concert = _concertRepository.FindBy(x => x.Id == request.Id && x.ReservationBusinessId == user.ReservationBusinessId).FirstOrDefault();
                if (concert != null)
                {
                    concert.DeletedDate = DateTime.UtcNow;
                    _concertRepository.Save();
                }
            }
        }
    }
}

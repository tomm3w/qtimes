using iVeew.common.api.Commands;
using IVeew.common.dal;
using QTimes.api.Exceptions;
using QTimes.core.dal.Enums;
using QTimes.core.dal.Models;
using System;
using System.Linq;

namespace QTimes.api.Commands.AddBusinessDetail
{
    public class AddBusinessDetailCommand : ICommand<AddBusinessDetailRequest>
    {
        private readonly IGenericRepository<QTimesContext, BusinessDetail> _businessRepository;
        private readonly IGenericRepository<QTimesContext, Concert> _concertRepository;
        private readonly IGenericRepository<QTimesContext, UserInfo> _userInfoRepository;

        public AddBusinessDetailCommand(IGenericRepository<QTimesContext, BusinessDetail> businessRepository,
            IGenericRepository<QTimesContext, UserInfo> userInfoRepository,
            IGenericRepository<QTimesContext, Concert> concertRepository)
        {
            _businessRepository = businessRepository;
            _userInfoRepository = userInfoRepository;
            _concertRepository = concertRepository;
        }

        public void Handle(AddBusinessDetailRequest request)
        {
            if (request.ReservationBusinessId == null || request.ReservationBusinessId == default(Guid))
            {
                var user = _userInfoRepository.FindBy(x => x.UserId == request.UserId).FirstOrDefault();
                if (user.ReservationBusinessId == null)
                    throw new NotFoundException();

                request.ReservationBusinessId = (Guid)user.ReservationBusinessId;
            }

            if (request.BusinessTypeId == (short)BusinessTypeEnum.EventConcert)
            {
                var concert = new Concert
                {
                    Id = Guid.NewGuid(),
                    Name = request.Name,
                    ReservationBusinessId = request.ReservationBusinessId,
                    Date = request.Date,
                    TimeFrom = request.TimeFrom,
                    TimeTo = request.TimeTo
                };
                _concertRepository.Add(concert);
                _concertRepository.Save();
            }
            else
            {
                var busnessi = new BusinessDetail
                {
                    Id = Guid.NewGuid(),
                    BusinessName = request.Name,
                    ReservationBusinessId = request.ReservationBusinessId,
                    BusinessTypeId = request.BusinessTypeId
                };
                _businessRepository.Add(busnessi);
                _businessRepository.Save();
            }
        }
    }
}
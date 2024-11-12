using common.api.Commands;
using common.dal;
using SeatQ.core.api.Exceptions;
using SeatQ.core.dal.Models;
using System;
using System.Linq;


namespace SeatQ.core.api.Commands.AddBusinessDetail
{
    public class AddBusinessDetailCommand : ICommand<AddBusinessDetailRequest>
    {
        private readonly IGenericRepository<SeatQEntities, BusinessDetail> _businessRepository;
        private readonly IGenericRepository<SeatQEntities, UserInfo> _userInfoRepository;

        public AddBusinessDetailCommand(IGenericRepository<SeatQEntities, BusinessDetail> businessRepository,
            IGenericRepository<SeatQEntities, UserInfo> userInfoRepository)
        {
            _businessRepository = businessRepository;
            _userInfoRepository = userInfoRepository;
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
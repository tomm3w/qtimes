using iVeew.common.api.Queries;
using IVeew.common.dal;
using QTimes.core.dal.Models;
using System;
using System.Linq;

namespace QTimes.api.Queries.Reservation.GetAccount
{
    public class GetAccountQuery : IQuery<GetAccountResponse, GetAccountRequest>
    {
        private readonly IGenericRepository<QTimesContext, UserInfo> _userInfoRepository;
        private readonly IGenericRepository<QTimesContext, ReservationBusiness> _businessRepository;

        public GetAccountQuery(IGenericRepository<QTimesContext, UserInfo> userInfoRepository,
            IGenericRepository<QTimesContext, ReservationBusiness> businessRepository)
        {
            _businessRepository = businessRepository;
            _userInfoRepository = userInfoRepository;
        }

        public GetAccountResponse Handle(GetAccountRequest request)
        {
            var user = _userInfoRepository.FindBy(x => x.UserId == request.UserId).FirstOrDefault();
            if (user.ReservationBusinessId == null)
                throw new Exception("Reservation business not registered.");

            var business = _businessRepository.FindBy(x => x.Id == user.ReservationBusinessId).FirstOrDefault();

            return new GetAccountResponse
            {
                UserName = request.UserName,
                Email = request.UserName,
                BusinessName = business.BusinessName,
                FullName = business.FullName,
                Address = business.Address,
                City = business.City,
                State = business.State,
                Zip = business.Zip,
                TimezoneOffset = business.TimezoneOffset,
                TimezoneOffsetValue = business.TimezoneOffsetValue
            };
        }
    }
}
using common.api.Queries;
using common.dal;
using SeatQ.core.dal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using WebGrease.Css.Extensions;

namespace SeatQ.core.api.Queries.Reservation.GetBusinessAccounts
{
    public class GetBusinessAccountsQuery : IQuery<GetBusinessAccountsResponse, GetBusinessAccountsRequest>
    {
        private readonly IGenericRepository<SeatQEntities, UserInfo> _userInfoRepository;
        private readonly IGenericRepository<SeatQEntities, Concert> _concertRepository;
        private readonly IGenericRepository<SeatQEntities, BusinessDetail> _businessRepository;

        public GetBusinessAccountsQuery(IGenericRepository<SeatQEntities, UserInfo> userInfoRepository,
        IGenericRepository<SeatQEntities, Concert> concertRepository,
        IGenericRepository<SeatQEntities, BusinessDetail> businessRepository)
        {
            _userInfoRepository = userInfoRepository;
            _concertRepository = concertRepository;
            _businessRepository = businessRepository;
        }

        public GetBusinessAccountsResponse Handle(GetBusinessAccountsRequest request)
        {
            var user = _userInfoRepository.FindBy(x => x.UserId == request.UserId).FirstOrDefault();
            if (user.ReservationBusinessId == null)
                throw new Exception("Reservation business not registered.");

            var business = _businessRepository.FindBy(x => x.ReservationBusinessId == user.ReservationBusinessId);

            var accounts = new List<BusinessAccount>();
            business.ForEach(x =>
            {
                accounts.Add(new BusinessAccount
                {
                    BusinessId = x.Id,
                    ReservationBusinessId = (Guid)x.ReservationBusinessId,
                    BusinessName = x.BusinessName,
                    LogoPath = x.LogoPath,
                    LogoFullPath = SeatQ.core.api.Helpers.ImageHelper.ContentAbsUrl("/Content/Uploads/Reservation/" + x.LogoPath ?? ""),
                    BusinessType = x.BusinessType.Name
                });
            });

            var concerts = _concertRepository.FindBy(x => x.ReservationBusinessId == user.ReservationBusinessId);

            concerts.ForEach(x =>
            {
                accounts.Add(new BusinessAccount
                {
                    ReservationBusinessId = x.ReservationBusinessId,
                    BusinessName = x.Name,
                    LogoPath = x.ImagePath,
                    LogoFullPath = SeatQ.core.api.Helpers.ImageHelper.ContentAbsUrl("/Content/Uploads/Reservation/" + x.ImagePath ?? ""),
                    ConcertId = x.Id,
                    BusinessType = "Concert"
                });
            });

            return new GetBusinessAccountsResponse { BusinessAccounts = accounts.OrderBy(x => x.BusinessType).ThenBy(x => x.BusinessName).ToList() };
        }
    }
}
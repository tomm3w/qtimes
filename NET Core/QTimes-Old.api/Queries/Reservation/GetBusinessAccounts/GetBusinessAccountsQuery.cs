using iVeew.common.api.Queries;
using IVeew.common.dal;
using Microsoft.AspNetCore.Http;
using QTimes.api.Infrastructure;
using QTimes.api.Models.Enums;
using QTimes.core.dal.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QTimes.api.Queries.Reservation.GetBusinessAccounts
{
    public class GetBusinessAccountsQuery : IQuery<GetBusinessAccountsResponse, GetBusinessAccountsRequest>
    {
        private readonly IGenericRepository<QTimesContext, UserInfo> _userInfoRepository;
        private readonly IGenericRepository<QTimesContext, Concert> _concertRepository;
        private readonly IGenericRepository<QTimesContext, BusinessDetail> _businessRepository;
        private readonly IHttpContextAccessor _accessor;
        private readonly IBlobService _blobService;

        public GetBusinessAccountsQuery(IGenericRepository<QTimesContext, UserInfo> userInfoRepository,
        IGenericRepository<QTimesContext, Concert> concertRepository,
        IGenericRepository<QTimesContext, BusinessDetail> businessRepository, IHttpContextAccessor accessor, IBlobService blobService)
        {
            _userInfoRepository = userInfoRepository;
            _concertRepository = concertRepository;
            _businessRepository = businessRepository;
            _accessor = accessor;
            _blobService = blobService;
        }

        public GetBusinessAccountsResponse Handle(GetBusinessAccountsRequest request)
        {
            var user = _userInfoRepository.FindBy(x => x.UserId == request.UserId).FirstOrDefault();
            if (user.ReservationBusinessId == null)
                throw new Exception("Reservation business not registered.");

            var business = _businessRepository.FindBy(x => x.ReservationBusinessId == user.ReservationBusinessId && x.DeletedDate == null);

            var accounts = new List<BusinessAccount>();
            business.ToList().ForEach(x =>
            {
                accounts.Add(new BusinessAccount
                {
                    BusinessId = x.Id,
                    ReservationBusinessId = (Guid)x.ReservationBusinessId,
                    BusinessName = x.BusinessName,
                    LogoPath = x.LogoPath,
                    LogoFullPath = _blobService.GetFileUrl(x.LogoPath) ?? "",
                    BusinessType = x.BusinessType.Name,
                    BusinessTypeId = (int)BusinessTypeEnum.Generic
                });
            });

            var concerts = _concertRepository.FindBy(x => x.ReservationBusinessId == user.ReservationBusinessId && x.DeletedDate == null);

            concerts.ToList().ForEach(x =>
            {
                accounts.Add(new BusinessAccount
                {
                    ReservationBusinessId = x.ReservationBusinessId,
                    BusinessName = x.Name,
                    LogoPath = x.ImagePath,
                    LogoFullPath = _blobService.GetFileUrl(x.ImagePath) ?? "",
                    ConcertId = x.Id,
                    BusinessType = BusinessTypeEnum.EventConcert.ToString(),
                    BusinessTypeId = (int)BusinessTypeEnum.EventConcert
                });
            });

            return new GetBusinessAccountsResponse { BusinessAccounts = accounts.OrderBy(x => x.BusinessType).ThenBy(x => x.BusinessName).ToList() };
        }
    }
}
using AutoMapper;
using iVeew.common.api.Queries;
using IVeew.common.dal;
using Microsoft.AspNetCore.Http;
using QTimes.api.Infrastructure;
using QTimes.core.dal.Models;
using System;
using System.Linq;

namespace QTimes.api.Queries.Reservation.GetBusinessProfile
{
    public class GetBusinessProfileQuery : IQuery<GetBusinessProfileResponse, GetBusinessProfileRequest>
    {
        private readonly IGenericRepository<QTimesContext, UserInfo> _userInfoRepository;
        private readonly IGenericRepository<QTimesContext, BusinessDetail> _reservationRepository;
        private readonly IBlobService _blobService;
        public GetBusinessProfileQuery(IGenericRepository<QTimesContext, UserInfo> userInfoRepository,
            IGenericRepository<QTimesContext, BusinessDetail> reservationRepository, IBlobService blobService)
        {
            _userInfoRepository = userInfoRepository;
            _reservationRepository = reservationRepository;
            _blobService = blobService;
        }

        public GetBusinessProfileResponse Handle(GetBusinessProfileRequest request)
        {

            var business = _reservationRepository.FindBy(x => x.Id == request.Id).FirstOrDefault();

            if (business == null)
                throw new Exception("Reservation business not registered.");

            var result = Mapper.Map<GetBusinessProfileResponse>(business);
            result.LogoFullPath = _blobService.GetFileUrl(business.LogoPath) ?? "";
            result.UserName = request.UserName;
            result.OpenHourFrom = result.OpenHourFrom != null ? result.OpenHourFrom.ToString() : "";
            result.OpenHourTo = result.OpenHourTo !=null ? result.OpenHourTo.ToString(): "";
            result.PrivacyPolicyFileFullPath = _blobService.GetFileUrl(result.PrivacyPolicyFilePath) ?? "";
            result.ServiceTermsFileFullPath = _blobService.GetFileUrl(result.ServiceTermsFilePath) ?? "";
            result.CummunityGuidelinesFileFullPath = _blobService.GetFileUrl(result.CummunityGuidelinesFilePath) ?? "";
            return result;
        }


    }
}
using AutoMapper;
using iVeew.common.api.Queries;
using IVeew.common.dal;
using QTimes.api.Infrastructure;
using QTimes.core.dal.Models;
using System;
using System.Linq;

namespace QTimes.api.Queries.Reservation.GetConcertById
{
    public class GetConcertByIdQuery : IQuery<GetConcertByIdResponse, GetConcertByIdRequest>
    {
        private readonly IGenericRepository<QTimesContext, Concert> _concertRepository;
        private IBlobService _blobService;

        public GetConcertByIdQuery(IGenericRepository<QTimesContext, Concert> concertRepository, IBlobService blobService)
        {
            _concertRepository = concertRepository;
            _blobService = blobService;
        }

        public GetConcertByIdResponse Handle(GetConcertByIdRequest request)
        {

            var business = _concertRepository.FindBy(x => x.Id == request.Id).FirstOrDefault();

            if (business == null)
                throw new Exception("Concert business not registered.");

            var result = Mapper.Map<GetConcertByIdResponse>(business);
            result.UserName = request.UserName;
            result.ImageFullPath = _blobService.GetFileUrl(result.ImagePath) ?? "";
            result.PrivacyPolicyFileFullPath = _blobService.GetFileUrl(result.PrivacyPolicyFilePath) ?? "";
            result.ServiceTermsFileFullPath = _blobService.GetFileUrl(result.ServiceTermsFilePath) ?? "";
            result.CummunityGuidelinesFileFullPath = _blobService.GetFileUrl(result.CummunityGuidelinesFilePath) ?? "";
            result.SeatMapFullPath = _blobService.GetFileUrl(result.SeatMapPath) ?? "";
            result.TimeFrom = result.TimeFrom;
            result.TimeTo = result.TimeTo;
            return result;
        }
    }
}
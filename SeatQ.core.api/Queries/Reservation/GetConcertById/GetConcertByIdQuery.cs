using AutoMapper;
using common.api.Queries;
using common.dal;
using SeatQ.core.dal.Models;
using System;
using System.Linq;

namespace SeatQ.core.api.Queries.Reservation.GetConcertById
{
    public class GetConcertByIdQuery : IQuery<GetConcertByIdResponse, GetConcertByIdRequest>
    {
        private readonly IGenericRepository<SeatQEntities, Concert> _concertRepository;

        public GetConcertByIdQuery(IGenericRepository<SeatQEntities, Concert> concertRepository)
        {
            _concertRepository = concertRepository;
        }

        public GetConcertByIdResponse Handle(GetConcertByIdRequest request)
        {

            var business = _concertRepository.FindBy(x => x.Id == request.Id).FirstOrDefault();

            if (business == null)
                throw new Exception("Concert business not registered.");

            var result = Mapper.Map<GetConcertByIdResponse>(business);
            result.UserName = request.UserName;
            result.ImageFullPath = SeatQ.core.api.Helpers.ImageHelper.ContentAbsUrl("/Content/Uploads/Reservation/" + result.ImagePath ?? "");
            result.PrivacyPolicyFileFullPath = SeatQ.core.api.Helpers.ImageHelper.ContentAbsUrl("/Content/Uploads/Reservation/" + result.PrivacyPolicyFilePath ?? "");
            result.ServiceTermsFileFullPath = SeatQ.core.api.Helpers.ImageHelper.ContentAbsUrl("/Content/Uploads/Reservation/" + result.ServiceTermsFilePath ?? "");
            result.CummunityGuidelinesFileFullPath = SeatQ.core.api.Helpers.ImageHelper.ContentAbsUrl("/Content/Uploads/Reservation/" + result.CummunityGuidelinesFilePath ?? "");
            result.SeatMapFullPath = SeatQ.core.api.Helpers.ImageHelper.ContentAbsUrl("/Content/Uploads/Reservation/" + result.SeatMapPath ?? "");
            return result;
        }
    }
}
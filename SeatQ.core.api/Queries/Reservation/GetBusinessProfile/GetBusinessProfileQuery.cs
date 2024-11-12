using AutoMapper;
using common.api.Queries;
using common.dal;
using SeatQ.core.dal.Models;
using System;
using System.Linq;

namespace SeatQ.core.api.Queries.Reservation.GetBusinessProfile
{
    public class GetBusinessProfileQuery : IQuery<GetBusinessProfileResponse, GetBusinessProfileRequest>
    {
        private readonly IGenericRepository<SeatQEntities, UserInfo> _userInfoRepository;
        private readonly IGenericRepository<SeatQEntities, BusinessDetail> _reservationRepository;

        public GetBusinessProfileQuery(IGenericRepository<SeatQEntities, UserInfo> userInfoRepository,
            IGenericRepository<SeatQEntities, BusinessDetail> reservationRepository)
        {
            _userInfoRepository = userInfoRepository;
            _reservationRepository = reservationRepository;
        }

        public GetBusinessProfileResponse Handle(GetBusinessProfileRequest request)
        {

            var business = _reservationRepository.FindBy(x => x.Id == request.Id).FirstOrDefault();

            if (business == null)
                throw new Exception("Reservation business not registered.");

            var result = Mapper.Map<GetBusinessProfileResponse>(business);
            result.LogoFullPath = SeatQ.core.api.Helpers.ImageHelper.ContentAbsUrl("/Content/Uploads/Reservation/" + business.LogoPath ?? "");
            result.UserName = request.UserName;
            return result;
        }


    }
}
using iVeew.common.api.Commands;
using IVeew.common.dal;
using QTimes.api.Exceptions;
using QTimes.core.dal.Models;
using System;
using System.Linq;

namespace QTimes.api.Commands.Reservation.UpdateConcertBusinessProfile
{
    public class UpdateConcertBusinessProfileCommand : ICommand<UpdateConcertBusinessProfileRequest>
    {
        private readonly IGenericRepository<QTimesContext, UserInfo> _userInfoRepository;
        private readonly IGenericRepository<QTimesContext, Concert> _concertRepository;

        public UpdateConcertBusinessProfileCommand(IGenericRepository<QTimesContext, UserInfo> userInfoRepository,
            IGenericRepository<QTimesContext, Concert> concertRepository)
        {
            _userInfoRepository = userInfoRepository;
            _concertRepository = concertRepository;
        }

        public void Handle(UpdateConcertBusinessProfileRequest request)
        {
            var user = _userInfoRepository.FindBy(x => x.UserId == request.UserId).FirstOrDefault();
            if (user.ReservationBusinessId == null)
                throw new Exception("Reservation business not registered.");

            request.ReservationBusinessId = (Guid)user.ReservationBusinessId;

            var concert = _concertRepository.FindBy(x => x.Id == request.ConcertId && x.ReservationBusinessId == request.ReservationBusinessId).FirstOrDefault();

            if (concert == null)
                throw new NotFoundException();

            concert.ImagePath = request.ImagePath;
            concert.Description = request.Description;
            concert.EnableCommunityGuidelines = request.EnableCommunityGuidelines;
            concert.EnablePrivacyPolicy = request.EnablePrivacyPolicy;
            concert.EnableServiceTerms = request.EnableServiceTerms;
            concert.PrivacyPolicyFilePath = request.PrivacyPolicyFilePath;
            concert.ServiceTermsFilePath = request.ServiceTermsFilePath;
            concert.CummunityGuidelinesFilePath = request.CummunityGuidelinesFilePath;
            //TODO: have to be part of ConcertEvent
            //if (!string.IsNullOrWhiteSpace(request.PassTemplateId))
            //{
            //    int outPasId;
            //    if(int.TryParse(request.PassTemplateId, out outPasId))
            //    {
            //        concert.PassTemplateId = outPasId;
            //    }
                
            //}
            concert.PhoneNo = request.PhoneNo;
            concert.MobileNo = request.MobileNo;
            concert.VirtualNo = request.VirtualNo;

            _concertRepository.Save();
        }
    }
}
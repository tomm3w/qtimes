using iVeew.common.api.Commands;
using IVeew.common.dal;
using QTimes.api.Exceptions;
using QTimes.core.dal.Models;
using System;
using System.Linq;

namespace QTimes.api.Commands.Reservation.UpdateConcertBusinessProfile
{
    public class UpdateConcertEventBusinessProfileCommand : ICommand<UpdateConcertEventBusinessProfileRequest>
    {
        private readonly IGenericRepository<QTimesContext, UserInfo> _userInfoRepository;
        private readonly IGenericRepository<QTimesContext, ConcertEvent> _concertRepository;

        public UpdateConcertEventBusinessProfileCommand(IGenericRepository<QTimesContext, UserInfo> userInfoRepository,
            IGenericRepository<QTimesContext, ConcertEvent> concertRepository)
        {
            _userInfoRepository = userInfoRepository;
            _concertRepository = concertRepository;
        }

        public void Handle(UpdateConcertEventBusinessProfileRequest request)
        {
            var user = _userInfoRepository.FindBy(x => x.UserId == request.UserId).FirstOrDefault();
            if (user.ReservationBusinessId == null)
                throw new Exception("Reservation business not registered.");

            request.ReservationBusinessId = (Guid)user.ReservationBusinessId;

            var concert = _concertRepository.FindBy(x => x.Id == request.Id && x.ConcertId == request.ConcertId).FirstOrDefault();

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
            concert.ShortUrl = request.ShortUrl;
            concert.PassTemplateId = request.PassTemplateId;
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
using common.api.Commands;
using common.dal;
using SeatQ.core.api.Exceptions;
using SeatQ.core.dal.Models;
using System;
using System.Linq;

namespace SeatQ.core.api.Commands.Reservation.UpdateConcertBusinessProfile
{
    public class UpdateConcertBusinessProfileCommand : ICommand<UpdateConcertBusinessProfileRequest>
    {
        private readonly IGenericRepository<SeatQEntities, UserInfo> _userInfoRepository;
        private readonly IGenericRepository<SeatQEntities, ReservationBusiness> _reservationRepository;
        private readonly IGenericRepository<SeatQEntities, Concert> _concertRepository;

        public UpdateConcertBusinessProfileCommand(IGenericRepository<SeatQEntities, UserInfo> userInfoRepository,
            IGenericRepository<SeatQEntities, ReservationBusiness> reservationRepository,
            IGenericRepository<SeatQEntities, Concert> concertRepository)
        {
            _userInfoRepository = userInfoRepository;
            _reservationRepository = reservationRepository;
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
            concert.PassTemplateId = request.PassTemplateId;
            concert.PhoneNo = request.PhoneNo;
            concert.MobileNo = request.MobileNo;
            concert.VirtualNo = request.VirtualNo;

            _concertRepository.Save();
        }
    }
}
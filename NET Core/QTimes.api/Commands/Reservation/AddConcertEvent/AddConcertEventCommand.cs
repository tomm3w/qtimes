using iVeew.common.api.Commands;
using IVeew.common.dal;
using QTimes.api.Exceptions;
using QTimes.core.dal.Models;
using System;
using System.Linq;

namespace QTimes.api.Commands.Reservation.AddConcertEvent
{
    public class AddConcertEventCommand : ICommand<AddConcertEventRequest>
    {
        private readonly IGenericRepository<QTimesContext, Concert> _concertRepository;
        private readonly IGenericRepository<QTimesContext, ConcertEvent> _concertEventRepository;

        public AddConcertEventCommand(IGenericRepository<QTimesContext, ConcertEvent> concertEventRepository,
            IGenericRepository<QTimesContext, Concert> concertRepository)
        {
            _concertEventRepository = concertEventRepository;
            _concertRepository = concertRepository;
        }
        public void Handle(AddConcertEventRequest request)
        {
            var concert = _concertRepository.FindBy(x => x.Id == request.ConcertId).FirstOrDefault();

            if (concert == null)
                throw new NotFoundException("Concert business not registered.");

            var concertEvent = new ConcertEvent
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Date = request.Date,
                TimeFrom = request.TimeFrom,
                TimeTo = request.TimeTo,
                ConcertId = request.ConcertId,
                ImagePath = concert.ImagePath,
                Description = concert.Description,
                VirtualNo = concert.VirtualNo,
                PhoneNo = concert.PhoneNo,
                MobileNo = concert.MobileNo,
                EnablePrivacyPolicy = concert.EnablePrivacyPolicy,
                PrivacyPolicyFilePath = concert.PrivacyPolicyFilePath,
                EnableCommunityGuidelines = concert.EnableCommunityGuidelines,
                CummunityGuidelinesFilePath = concert.CummunityGuidelinesFilePath,
                EnableServiceTerms = concert.EnableServiceTerms,
                ServiceTermsFilePath = concert.ServiceTermsFilePath
            };

            _concertEventRepository.Add(concertEvent);
            _concertEventRepository.Save();
        }
    }
}
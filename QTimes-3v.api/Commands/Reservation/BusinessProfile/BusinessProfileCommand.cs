using iVeew.common.api.Commands;
using IVeew.common.dal;
using QTimes.core.dal.Models;
using System;
using System.Linq;

namespace QTimes.api.Commands.Reservation.BusinessProfile
{
    public class BusinessProfileCommand : ICommand<BusinessProfileRequest>
    {
        private readonly IGenericRepository<QTimesContext, BusinessDetail> _businessRepository;

        public BusinessProfileCommand(IGenericRepository<QTimesContext, BusinessDetail> businessRepository)
        {
            _businessRepository = businessRepository;
        }

        public void Handle(BusinessProfileRequest request)
        {
            var businessDetail = _businessRepository.FindBy(x => x.Id == request.Id).FirstOrDefault();
            if (businessDetail == null)
                throw new Exception("Reservation business not registered.");

            businessDetail.PassTemplateId = request.PassTemplateId;
            businessDetail.VirtualNo = request.VirtualNo;
            businessDetail.PhoneNo = request.PhoneNo;
            businessDetail.MobileNo = request.MobileNo;
            businessDetail.ShortUrl = request.ShortUrl;
            businessDetail.LogoPath = request.LogoPath;
            businessDetail.BusinessName = request.BusinessDetailName;
            businessDetail.Description = request.Description;
            businessDetail.EnableCommunityGuidelines = request.EnableCommunityGuidelines;
            businessDetail.EnablePrivacyPolicy = request.EnablePrivacyPolicy;
            businessDetail.EnableServiceTerms = request.EnableServiceTerms;
            businessDetail.PrivacyPolicyFilePath = request.PrivacyPolicyFilePath;
            businessDetail.ServiceTermsFilePath = request.ServiceTermsFilePath;
            businessDetail.CummunityGuidelinesFilePath = request.CummunityGuidelinesFilePath;
            _businessRepository.Save();
        }
    }
}
using common.api.Commands;
using common.dal;
using SeatQ.core.dal.Models;
using System;
using System.Linq;

namespace SeatQ.core.api.Commands.Reservation.BusinessProfile
{
    public class BusinessProfileCommand : ICommand<BusinessProfileRequest>
    {
        private readonly IGenericRepository<SeatQEntities, ReservationBusiness> _reservationRepository;
        private readonly IGenericRepository<SeatQEntities, BusinessDetail> _businessRepository;

        public BusinessProfileCommand(IGenericRepository<SeatQEntities, ReservationBusiness> reservationRepository,
            IGenericRepository<SeatQEntities, BusinessDetail> businessRepository)
        {
            _reservationRepository = reservationRepository;
            _businessRepository = businessRepository;
        }

        public void Handle(BusinessProfileRequest request)
        {
            var businessDetail = _businessRepository.FindBy(x => x.Id == request.Id).FirstOrDefault();
            if (businessDetail == null)
                throw new Exception("Reservation business not registered.");

            var business = _reservationRepository.FindBy(x => x.Id == businessDetail.ReservationBusinessId).FirstOrDefault();

            if (business == null)
                throw new Exception("Reservation business not registered.");

            business.BusinessName = request.BusinessName;
            business.FullName = request.FullName;
            business.Email = request.Email;
            business.Address = request.Address;
            business.City = request.City;
            business.State = request.State;
            business.Zip = request.Zip;
            business.TimezoneOffset = request.TimezoneOffset;
            business.TimezoneOffsetValue = request.TimezoneOffsetValue;
            _reservationRepository.Save();

            businessDetail.PassTemplateId = request.PassTemplateId;
            businessDetail.VirtualNo = request.VirtualNo;
            businessDetail.PhoneNo = request.PhoneNo;
            businessDetail.MobileNo = request.MobileNo;
            businessDetail.ShortUrl = request.ShortUrl;
            businessDetail.LogoPath = request.LogoPath;
            _businessRepository.Save();
        }
    }
}
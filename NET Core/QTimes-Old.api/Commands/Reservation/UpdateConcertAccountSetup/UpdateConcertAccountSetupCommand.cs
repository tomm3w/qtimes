using iVeew.common.api.Commands;
using IVeew.common.dal;
using Microsoft.AspNetCore.Identity;
using QTimes.core.dal.Models;
using System;
using System.Linq;

namespace QTimes.api.Commands.Reservation.UpdateConcertAccountSetup
{
    public class UpdateConcertAccountSetupCommand : ICommand<UpdateConcertAccountSetupRequest>
    {
        private readonly IGenericRepository<QTimesContext, UserInfo> _userInfoRepository;
        private readonly IGenericRepository<QTimesContext, ReservationBusiness> _reservationRepository;
        private readonly IGenericRepository<QTimesContext, Concert> _concertRepository;
        private readonly UserManager<IdentityUser> _userManager;
        public UpdateConcertAccountSetupCommand(IGenericRepository<QTimesContext, UserInfo> userInfoRepository,
            IGenericRepository<QTimesContext, ReservationBusiness> reservationRepository, IGenericRepository<QTimesContext, Concert> concertRepository, UserManager<IdentityUser> userManager)
        {
            _userInfoRepository = userInfoRepository;
            _reservationRepository = reservationRepository;
            _concertRepository = concertRepository;
            _userManager = userManager;
        }

        public void Handle(UpdateConcertAccountSetupRequest request)
        {

            var user = _userInfoRepository.FindBy(x => x.UserId == request.UserId).FirstOrDefault();
            if (user.ReservationBusinessId == null)
                throw new Exception("Reservation business not registered.");

            var business = _reservationRepository.FindBy(x => x.Id == user.ReservationBusinessId).FirstOrDefault();
            var concert = _concertRepository.FindBy(x => x.ReservationBusinessId == user.ReservationBusinessId).FirstOrDefault();
            if (business == null)
                throw new Exception("Reservation business not registered.");

            business.BusinessName = request.BusinessName;
            business.FullName = request.FullName;
            business.Email = request.Email;
            business.Address = request.Address;
            business.City = request.City;
            business.State = request.State;
            business.Zip = request.Zip;

            _reservationRepository.Save();
            if (concert != null)
            {
                concert.PhoneNo = request.PhoneNo;
                concert.MobileNo = request.MobileNo;
                concert.VirtualNo = request.VirtualNo;
                _concertRepository.Save();
            }
            using (QTimesContext context = new QTimesContext())
            {
                var usr = context.AspNetUsers.FirstOrDefault(x => x.Id.ToLower() == request.UserId.ToString().ToLower());
                if (usr != null)
                {
                    usr.Email = request.Email;
                    context.AspNetUsers.Update(usr);
                    context.SaveChanges();
                }
            }
            if (!string.IsNullOrEmpty(request.NewPassword))
            {
                var identity = _userManager.FindByIdAsync(request.UserId.ToString()).GetAwaiter().GetResult();
                var result = _userManager.ChangePasswordAsync(identity, request.CurrentPassword, request.NewPassword).GetAwaiter().GetResult();

                if (!result.Succeeded)
                    throw new Exception("Password change failed.");
            }
        }
    }
}
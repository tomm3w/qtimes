using common.api.Commands;
using common.dal;
using SeatQ.core.dal.Models;
using System;
using System.Linq;
using System.Web.Security;

namespace SeatQ.core.api.Commands.Reservation.UpdateConcertAccountSetup
{
    public class UpdateConcertAccountSetupCommand : ICommand<UpdateConcertAccountSetupRequest>
    {
        private readonly IGenericRepository<SeatQEntities, UserInfo> _userInfoRepository;
        private readonly IGenericRepository<SeatQEntities, ReservationBusiness> _reservationRepository;

        public UpdateConcertAccountSetupCommand(IGenericRepository<SeatQEntities, UserInfo> userInfoRepository,
            IGenericRepository<SeatQEntities, ReservationBusiness> reservationRepository)
        {
            _userInfoRepository = userInfoRepository;
            _reservationRepository = reservationRepository;
        }

        public void Handle(UpdateConcertAccountSetupRequest request)
        {

            var user = _userInfoRepository.FindBy(x => x.UserId == request.UserId).FirstOrDefault();
            if (user.ReservationBusinessId == null)
                throw new Exception("Reservation business not registered.");

            var business = _reservationRepository.FindBy(x => x.Id == user.ReservationBusinessId).FirstOrDefault();

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

            var usr = Membership.GetUser(request.UserId);
            if (usr != null)
            {
                usr.Email = request.Email;
                Membership.UpdateUser(usr);
            }

            if (!string.IsNullOrEmpty(request.NewPassword))
            {
                var result = usr.ChangePassword(request.CurrentPassword, request.NewPassword);
                if (!result)
                    throw new Exception("Password change failed.");
            }
        }
    }
}
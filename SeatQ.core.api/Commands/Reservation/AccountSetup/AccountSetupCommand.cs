using common.api.Commands;
using common.dal;
using SeatQ.core.dal.Models;
using System;
using System.Linq;
using System.Web.Security;

namespace SeatQ.core.api.Commands.Reservation.AccountSetup
{
    public class AccountSetupCommand : ICommand<AccountSetupRequest>
    {
        private readonly IGenericRepository<SeatQEntities, UserInfo> _userInfoRepository;

        public AccountSetupCommand(IGenericRepository<SeatQEntities, UserInfo> userInfoRepository)
        {
            _userInfoRepository = userInfoRepository;
        }

        public void Handle(AccountSetupRequest request)
        {
            var user = _userInfoRepository.FindBy(x => x.UserId == request.UserId).FirstOrDefault();
            if (user.ReservationBusinessId == null)
                throw new Exception("Reservation business not registered.");

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
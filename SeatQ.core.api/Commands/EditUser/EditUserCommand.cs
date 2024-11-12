using common.api.Commands;
using common.dal;
using SeatQ.core.dal.Models;
using System.Linq;

namespace iDestn.core.api.Commands.EditUser
{

    public class EditUserCommand : ICommand<EditUserRequest>
    {
        private readonly IGenericRepository<SeatQEntities, UserProfile> _userRepository;
        public EditUserCommand(IGenericRepository<SeatQEntities, UserProfile> userRepository)
        {
            _userRepository = userRepository;
        }

        public void Handle(EditUserRequest request)
        {

            UserProfile usr = null;
            using (SeatQEntities context = new SeatQEntities())
            {
                usr = context.UserProfiles.FirstOrDefault(u => u.UserId == request.Id);

                if (usr != null)
                {

                    context.UpdateUserEmail(request.Id, request.Email);
                    usr.isUserActive = request.IsActive;
                    usr.StaffTypeId = (short)request.StaffTypeId;

                    context.SaveChanges();

                    if (!string.IsNullOrEmpty(request.NewPassword))
                    {
                        var userForUpdate = System.Web.Security.Membership.GetUser(request.Username);

                        userForUpdate.ChangePassword(request.Password, request.NewPassword);
                        System.Web.Security.Membership.UpdateUser(userForUpdate);
                    }

                }
            }
        }
    }
}
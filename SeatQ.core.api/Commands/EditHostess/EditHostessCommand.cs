using common.api.Commands;
using common.dal;
using SeatQ.core.api.Exceptions;
using SeatQ.core.dal.Models;
using System.Linq;

namespace SeatQ.core.api.Commands.EditHostess
{
    public class EditHostessCommand : ICommand<EditHostessRequest>
    {
        private readonly IGenericRepository<SeatQEntities, UserProfile> _hostessRepository;
        private readonly IGenericRepository<SeatQEntities, UserInfo> _userInfoRepository;
        public EditHostessCommand(IGenericRepository<SeatQEntities, UserProfile> hostessRepository, IGenericRepository<SeatQEntities, UserInfo> userInfoRepository)
        {
            _hostessRepository = hostessRepository;
            _userInfoRepository = userInfoRepository;
        }

        public void Handle(EditHostessRequest request)
        {
            var user = _hostessRepository.FindBy(x => x.UserId == request.Model.UserId).FirstOrDefault();
            if (user != null)
            {
                //password edit
                if (!string.IsNullOrEmpty(request.Model.Password))
                {
                    var userForUpdate = System.Web.Security.Membership.GetUser(request.Model.UserName);

                    userForUpdate.ChangePassword(userForUpdate.ResetPassword(), request.Model.Password);
                    System.Web.Security.Membership.UpdateUser(userForUpdate);
                }


                var userInfo = _userInfoRepository.FindBy(x => x.UserId == request.Model.UserId).FirstOrDefault();
                if (userInfo != null)
                {
                    userInfo.isActive = request.Model.IsActive;
                    userInfo.isDeleted = request.Model.IsDeleted;
                    userInfo.StaffTypeId = (short)request.Model.StaffTypeId;
                    _userInfoRepository.Edit(userInfo);
                    _userInfoRepository.Save();
                }
            }
        }

        private void ValidateRequest(EditHostessRequest request)
        {
            if (_hostessRepository.Any(x => x.Email == request.Model.Email && x.UserId != request.Model.UserId))
            {
                throw new EmailDuplicationException();
            }
        }
    }
}
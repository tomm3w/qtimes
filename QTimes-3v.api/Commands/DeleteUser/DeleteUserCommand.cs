using iVeew.common.api.Commands;
using IVeew.common.dal;
using QTimes.core.dal.Models;
using System.Linq;

namespace QTimes.api.Commands.DeleteUser
{
    public class DeleteUserCommand : ICommand<DeleteUserRequest>
    {
        private readonly IGenericRepository<QTimesContext, UserInfo> _userInfoRepository;
        public DeleteUserCommand(IGenericRepository<QTimesContext, UserInfo> userInfoRepository)
        {
            _userInfoRepository = userInfoRepository;
        }

        public void Handle(DeleteUserRequest request)
        {
            var user = _userInfoRepository.FindBy(x => x.UserId == request.Id).FirstOrDefault();
            if (user != null)
            {
                user.IsDeleted = true;
                _userInfoRepository.Edit(user);
                _userInfoRepository.Save();
            }
        }
    }
}
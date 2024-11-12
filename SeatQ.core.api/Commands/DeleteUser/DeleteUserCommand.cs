using common.api.Commands;
using common.dal;
using SeatQ.core.dal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SeatQ.core.api.Commands.DeleteUser
{
    public class DeleteUserCommand : ICommand<DeleteUserRequest>
    {
        private readonly IGenericRepository<SeatQEntities, UserInfo> _userInfoRepository;
        public DeleteUserCommand(IGenericRepository<SeatQEntities, UserInfo> userInfoRepository)
        {
            _userInfoRepository = userInfoRepository;
        }

        public void Handle(DeleteUserRequest request)
        {
            var user = _userInfoRepository.FindBy(x => x.UserId == request.Id).FirstOrDefault();
            if (user != null)
            {
                user.isDeleted = true;
                _userInfoRepository.Edit(user);
                _userInfoRepository.Save();
            }
        }
    }
}
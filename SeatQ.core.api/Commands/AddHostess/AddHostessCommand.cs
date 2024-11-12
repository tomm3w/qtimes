using common.api.Commands;
using common.dal;
using SeatQ.core.api.Exceptions;
using SeatQ.core.dal.Models;
using System;

namespace SeatQ.core.api.Commands.AddHostess
{
    public class AddHostessCommand : ICommand<AddHostessRequest>
    {
        private readonly IGenericRepository<SeatQEntities, UserProfile> _hostessRepository;
        private readonly IGenericRepository<SeatQEntities, UserInfo> _userInfoRepository;
        private readonly IGenericRepository<SeatQEntities, UsersInRestaurant> _usersInRestaurantRepository;
        public AddHostessCommand(IGenericRepository<SeatQEntities, UserProfile> hostessRepository, IGenericRepository<SeatQEntities, UserInfo> userInfoRepository, IGenericRepository<SeatQEntities, UsersInRestaurant> usersInRestaurantRepository)
        {
            _hostessRepository = hostessRepository;
            _userInfoRepository = userInfoRepository;
            _usersInRestaurantRepository = usersInRestaurantRepository;
        }

        public void Handle(AddHostessRequest request)
        {
            ValidateRequest(request);
            var user = System.Web.Security.Membership.CreateUser(request.Model.UserName, request.Model.Password, request.Model.Email);
            // get roles from db 
            System.Web.Security.Roles.AddUserToRoles(user.UserName, new[] { "User", "IdentityServerUsers" });

            //create userData
            var data = new UserInfo
            {
                UserId = (Guid)user.ProviderUserKey,
                RestaurantChainId = request.Model.RestaurantChainId,
                isActive = true,
                isDeleted = false,
                StaffTypeId = (short)request.Model.StaffTypeId
            };
            _userInfoRepository.Add(data);
            _userInfoRepository.Save();

            var uInfo = new UsersInRestaurant
            {
                UserId = (Guid)user.ProviderUserKey,
                RestaurantChainId = request.Model.RestaurantChainId
            };
            _usersInRestaurantRepository.Add(uInfo);
            _usersInRestaurantRepository.Save();

        }

        private void ValidateRequest(AddHostessRequest request)
        {
            if (_hostessRepository.Any(x => x.UserName == request.Model.UserName))
            {
                throw new UsernameDuplicationException();
            }

            if (_hostessRepository.Any(x => x.Email == request.Model.Email))
            {
                throw new EmailDuplicationException();
            }
        }
    }
}
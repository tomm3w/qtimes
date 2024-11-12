using System.Net;
using System.Net.Http;
using System.Web.Http;
using common.api.Infrastructure;
using common.api;
using SeatQ.core.api.Queries.GetUsers;
using System.Web.Http.Cors;
using SeatQ.core.api.Queries.GetUserByName;
using SeatQ.core.api.Queries.GetUserById;
using System;
using SeatQ.core.dal.Infrastructure.Repositories;
using SeatQ.core.dal.Models;
using userinfo.dal.Repositories;

namespace SeatQ.core.api.Controllers
{
    public class UsersController : SpecificApiController
    {

        private readonly IUserRepository _userRepository;
        private readonly IAccountInfoRepository _accountInfoRepository;

        internal class UsersResponse
        {
            public UsersResponse(GetUsersResponse users)
            {
                this.users = users;
            }

            public GetUsersResponse users { get; private set; }
        }


        public UsersController(IApplicationFacade facade, IUserRepository userRepository, IAccountInfoRepository accountInfoRepository)
            : base(facade)
        {
            _userRepository = userRepository;
            _accountInfoRepository = accountInfoRepository;
        }


        // GET api/users
        [Route("api/users/")]
        [Authorize]
        [Authorize(Roles = "Administrator,Regional Manager")]
        [EnableCors(origins: "*", headers: "*", methods: "*", SupportsCredentials = true)]
        public HttpResponseMessage Get()
        {
            //var user = _userRepository.GetAllUsers();
            //List<UserResponse> responses = user.Select(ToResponseUser).ToList();
            //return new HttpResponseMessage(HttpStatusCode.OK) { Content = new JsonContent(responses) };

            var response = GetUsers();
            return new HttpResponseMessage(HttpStatusCode.OK) { Content = new JsonContent(response) };
        }

        private UsersResponse GetUsers()
        {
            UsersResponse response = null;
            var usersRequest = new GetUsersRequest();
            var users = Facade.Query<GetUsersResponse, GetUsersRequest>(usersRequest);
            response = new UsersResponse(users);
            return response;
        }


        // GET api/users/alen
        [Route("api/users/{username}")]
        [Authorize]
        [Authorize(Roles = "Administrator,Regional Manager")]
        [EnableCors(origins: "*", headers: "*", methods: "*", SupportsCredentials = true)]
        public HttpResponseMessage Get(string username)
        {

            var request = new GetUserByNameRequest(username);
            var user = Facade.Query<GetUserByNameResponse, GetUserByNameRequest>(request);

            if (user != null)
            {
                return new HttpResponseMessage(HttpStatusCode.OK) { Content = new JsonContent(user) };
            }
            return new HttpResponseMessage(HttpStatusCode.NotFound);
        }



        // GET api/users/1
        [Route("api/users/")]
        [Authorize]
        [Authorize(Roles = "Administrator,Regional Manager")]
        [EnableCors(origins: "*", headers: "*", methods: "*", SupportsCredentials = true)]
        public HttpResponseMessage Get(Guid userid)
        {
            /*var user = _userRepository.GetUserByUserId(userid);
            if (user != null)
            {
                var jsonResult = new
                {
                    RestaurantChainId = user.RestaurantChainId,
                    UserId = user.UserId,
                    UserName = user.UserName,
                    Email = user.Email,
                    isActive = user.isUserActive
                };

                return new HttpResponseMessage(HttpStatusCode.OK) { Content = new JsonContent(jsonResult) };
            }

            return new HttpResponseMessage(HttpStatusCode.NotFound);
            */

            var request = new GetUserByIdRequest(userid);
            var user = Facade.Query<GetUserByIdResponse, GetUserByIdRequest>(request);
            if (user != null)
            {
                var jsonResult = new
                {
                    RestaurantChainId = user.User.RestaurantChainId,
                    UserId = user.User.UserId,
                    UserName = user.User.UserName,
                    Email = user.User.Email,
                    isActive = user.User.isUserActive,
                    StaffTypeId = user.User.StaffTypeId
                };
                return CreateHttpResponse(jsonResult);
            }

            return new HttpResponseMessage(HttpStatusCode.NotFound);
        }

        // GET api/users/1
        [Route("api/users/GetCurrentUser")]
        [Authorize]
        [Authorize(Roles = "Administrator,Regional Manager")]
        [EnableCors(origins: "*", headers: "*", methods: "*", SupportsCredentials = true)]
        [HttpGet]
        public HttpResponseMessage GetCurrentUser(int RestaurantChainId)
        {
            if (ModelState.IsValid)
            {
                var userId = _userRepository.GetUserId(Username);
                var request = new GetUserByIdRequest(userId);
                var user = Facade.Query<GetUserByIdResponse, GetUserByIdRequest>(request);
                if (user != null)
                {
                    var jsonResult = new
                    {
                        RestaurantChainId = user.User.RestaurantChainId,
                        UserId = user.User.UserId,
                        UserName = user.User.UserName,
                        Email = user.User.Email,
                        isActive = user.User.isUserActive
                    };
                    return CreateHttpResponse(jsonResult);
                }
            }
            return new HttpResponseMessage(HttpStatusCode.NotFound);
        }

    }
}
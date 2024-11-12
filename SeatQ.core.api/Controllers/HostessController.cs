using common.api;
using common.api.Infrastructure;
using common.api.Infrastructure.Exeptions;
using Core.Extensions;
using SeatQ.core.api.Commands.AddHostess;
using SeatQ.core.api.Commands.EditHostess;
using SeatQ.core.api.Exceptions;
using SeatQ.core.api.Queries.GetHostess;
using SeatQ.core.api.Queries.GetStaffTypes;
using SeatQ.core.dal.Infrastructure.Repositories;
using SeatQ.core.dal.Models;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using userinfo.dal.Repositories;

namespace SeatQ.core.api.Controllers
{
    [Filters.InvalidPostDataExceptionFilter]
    public class HostessController : SpecificApiController
    {

        private readonly IAccountInfoRepository _accountInfoRepository;
        public HostessController(IApplicationFacade applicationFacade, IUserRepository userRepository, IAccountInfoRepository accountInfoRepository)
            : base(applicationFacade)
        {
            _accountInfoRepository = accountInfoRepository;
        }

        /// <summary>
        /// Get Restaurant Hostess
        /// </summary>
        /// <param name="RestaurantChainId"></param>
        /// <returns></returns>
        [Route("api/hostess/")]
        [Authorize(Roles = "Administrator,Regional Manager")]
        [EnableCors(origins: "*", headers: "*", methods: "*", SupportsCredentials = true)]
        public HttpResponseMessage Get(int RestaurantChainId)
        {
            if (ModelState.IsValid)
            {
                var hostess = _accountInfoRepository.GetHostess(RestaurantChainId);

                var response = hostess.Select(u => new
                {
                    UserId = u.UserId,
                    UserName = u.UserName,
                    Email = u.Email,
                    CreateDate = u.CreateDate.ToUniversalTimeString(),
                    LastAccessDateTime = u.LastAccessDateTime.ToUniversalTimeString(),
                    isActive = u.isActive,
                    StaffTypeId = u.StaffTypeId,
                    Title = u.Title
                });

                return CreateHttpResponse(hostess);
            }
            throw new InvalidPostDataException(ModelState);
        }

        /// <summary>
        /// Add Restaurant Hostess
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("api/hostess/")]
        [Authorize(Roles = "Administrator,Regional Manager")]
        [EnableCors(origins: "*", headers: "*", methods: "*", SupportsCredentials = true)]
        public HttpResponseMessage Post(HostessModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var request = new AddHostessRequest(model);
                    Facade.Command(request);

                    var req = new GetHostessRequest((int)request.Model.RestaurantChainId);
                    var response = Facade.Query<GetHostessResponse, GetHostessRequest>(req);

                    var result = response.Users.Select(u => new
                    {
                        UserId = u.UserId,
                        UserName = u.UserName,
                        Email = u.Email,
                        CreateDate = u.CreateDate.ToUniversalTimeString(),
                        LastAccessDateTime = u.LastAccessDateTime.ToUniversalTimeString(),
                        isActive = u.isUserActive
                    });

                    return CreateHttpResponse(result);
                }
                catch (ApiException exception)
                {
                    return CreateHttpResponse(exception);
                }
            }
            throw new InvalidPostDataException(ModelState);
        }

        /// <summary>
        /// Update Restaurant Hostess
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("api/hostess/")]
        [Authorize(Roles = "Administrator,Regional Manager")]
        [EnableCors(origins: "*", headers: "*", methods: "*", SupportsCredentials = true)]
        public HttpResponseMessage Put(HostessModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var request = new EditHostessRequest(model);
                    Facade.Command(request);

                    //_accountInfoRepository.UpdateUserEmail((Guid)request.Model.UserId, request.Model.Email);

                    var req = new GetHostessRequest((int)model.RestaurantChainId);
                    var response = Facade.Query<GetHostessResponse, GetHostessRequest>(req);

                    var result = response.Users.Select(u => new
                    {
                        UserId = u.UserId,
                        UserName = u.UserName,
                        Email = u.Email,
                        CreateDate = u.CreateDate.ToUniversalTimeString(),
                        LastAccessDateTime = u.LastAccessDateTime.ToUniversalTimeString(),
                        isActive = u.isUserActive
                    });

                    return CreateHttpResponse(result);
                }
                catch (ApiException exception)
                {
                    return CreateHttpResponse(exception);
                }
            }
            throw new InvalidPostDataException(ModelState);
        }

        /// <summary>
        /// Get Staff Types
        /// </summary>
        [Route("api/stafftypes/")]
        [Authorize(Roles = "Administrator,Regional Manager")]
        [EnableCors(origins: "*", headers: "*", methods: "*", SupportsCredentials = true)]
        public HttpResponseMessage GetStaffTypes()
        {
            var response = Facade.Query<GetStaffTypesResponse, GetStaffTypesRequest>(new GetStaffTypesRequest());
            return CreateHttpResponse(response);
        }
    }
}
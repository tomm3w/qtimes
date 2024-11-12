using common.api;
using common.api.Infrastructure;
using SeatQ.core.api.Exceptions;
using SeatQ.core.dal.Infrastructure.Repositories;
using SeatQ.core.dal.Models;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace SeatQ.core.api.Controllers
{
    [Filters.InvalidPostDataExceptionFilter]
    public class AccountInfoController : SpecificApiController
    {
        private readonly IAccountInfoRepository _accountInfoRepository;
        public AccountInfoController(IApplicationFacade applicationFacade, IAccountInfoRepository accountInfoRepository)
            : base(applicationFacade)
        {
            _accountInfoRepository = accountInfoRepository;
        }



        /// <summary>
        /// Update Account Info
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("api/accountinfo/")]
        [Authorize(Roles = "Administrator, Regional Manager")]
        [EnableCors(origins: "*", headers: "*", methods: "*", SupportsCredentials = true)]
        public HttpResponseMessage Put(AccountInfoModel model)
        {
            if (ModelState.IsValid)
            {
                var usr = _accountInfoRepository.UpdateAccountInfo(model);
                if (usr != null)
                {
                    var jsonResult = new
                    {
                        UserId = usr.UserId,
                        RestaurantChainId = usr.RestaurantChainId,
                        UserName = usr.UserName,
                        Email = usr.Email,
                        IsActive = usr.isUserActive
                    };
                    return CreateHttpResponse(jsonResult);
                }
                else
                {
                    var jsonResult = new
                        {
                            Message = "User not found."
                        };
                    return CreateHttpResponse(jsonResult);
                }
            }
            throw new InvalidPostDataException(ModelState);
        }


        /// <summary>
        /// Update Account Info
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("api/accountinfo/")]
        [Authorize(Roles = "Administrator, Regional Manager")]
        [EnableCors(origins: "*", headers: "*", methods: "*", SupportsCredentials = true)]
        public HttpResponseMessage Delete(AccountInfoModel model)
        {
            if (ModelState.IsValid)
            {
                var usr = _accountInfoRepository.DeleteAccountInfo(model);
                if (usr != null)
                {
                    var jsonResult = new
                    {
                        UserId = usr.UserId,
                        RestaurantChainId = usr.RestaurantChainId,
                        IsActive = usr.isActive,
                        IsDeleted = usr.isDeleted
                    };
                    return CreateHttpResponse(jsonResult);
                }
                else
                {
                    var jsonResult = new
                    {
                        Message = "User not found."
                    };
                    return CreateHttpResponse(jsonResult);
                }
            }
            throw new InvalidPostDataException(ModelState);
        }

        /// <summary>
        /// Update Account Preferences
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("api/preferences/")]
        [Authorize(Roles = "Administrator, Regional Manager")]
        [EnableCors(origins: "*", headers: "*", methods: "*", SupportsCredentials = true)]
        public HttpResponseMessage Put(PreferencesModel model)
        {
            if (ModelState.IsValid)
            {
                var res = _accountInfoRepository.UpdatePreferences(model);
                if (res != null)
                {
                    var jsonResult = new
                    {
                        RestaurantChainId = res.RestaurantChainId
                    };
                    return CreateHttpResponse(jsonResult);
                }
                else
                {
                    var jsonResult = new
                    {
                        Message = "Restaurant not found."
                    };
                    return CreateHttpResponse(jsonResult);
                }
            }
            throw new InvalidPostDataException(ModelState);
        }

    }
}
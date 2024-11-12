using SeatQ.core.api.Infrastructure;
using SeatQ.core.dal.Infrastructure.Repositories;
using SeatQ.core.dal.Models;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using iDestn.core.api.Models;
using System.Web.Http.Cors;
using common.api.Infrastructure;
using common.api;
using SeatQ.core.api.Queries.GetRestaurant;
using SeatQ.core.api.Queries.GetRestaurantChain;
using SeatQ.core.api.Commands.EditRestaurantChain;
using SeatQ.core.api.Exceptions;
using Core.Extensions;
using System;
using System.Web;
using SeatQ.core.api.Models;
using SeatQ.core.api.Helpers;
using Authentication.Helpers;
using SeatQ.core.api.Queries.GetAllRestaurant;
using System.Linq;
using SeatQ.core.api.Commands.AddAccount;
using SeatQ.core.api.Queries.GetRestaurantChains;
using SeatQ.core.api.Commands.AddRestaurantBusiness;
using common.api.Infrastructure.Exeptions;
using SeatQ.core.api.Queries.GetRestaurantBusiness;
using userinfo.dal.Repositories;
using SeatQ.core.api.Queries.GetUserRestaurants;
using SeatQ.core.api.Queries.GetRestaurantByBusinessId;
using SeatQ.core.api.Commands.AddWaitList;
using SeatQ.core.api.Queries.GetGuestType;
using System.Web.Http.Description;
using SeatQ.core.api.Commands.AssignStaff;

namespace SeatQ.core.api.Controllers
{
    [Filters.InvalidPostDataExceptionFilter]
    public class RestaurantController : SpecificApiController
    {
        private readonly IUserRepository _userRepository;
        private readonly IAccountInfoRepository _accountInfoRepository;
        private readonly IWaitListRepository _waitListRepository;
        public RestaurantController(IApplicationFacade facade, IWaitListRepository waitListRepository, IUserRepository userRepository, IAccountInfoRepository accountInfoRepository)
            : base(facade)
        {
            _waitListRepository = waitListRepository;
            _userRepository = userRepository;
            _accountInfoRepository = accountInfoRepository;
        }

        /// <summary>
        /// Get Restaurant Info
        /// </summary>
        /// <param name="restaurantid">The ID of the restaurant</param>
        /// <returns></returns>
        [Route("api/restaurant/")]
        [Authorize(Roles = "Administrator,Regional Manager")]
        [EnableCors(origins: "*", headers: "*", methods: "*", SupportsCredentials = true)]
        public HttpResponseMessage Get(int RestaurantChainId)
        {

            var request = new GetRestaurantChainRequest(RestaurantChainId);
            var r = Facade.Query<GetRestaurantChainResponse, GetRestaurantChainRequest>(request);
            if (r != null)
            {
                var jsonResult = new
                {
                    RestaurantId = r.RestaurantChain.RestaurantId,
                    RestaurantChainId = r.RestaurantChain.RestaurantChainId,
                    BusinessName = r.RestaurantChain.Restaurant.BusinessName,
                    LogoPath = r.RestaurantChain.Restaurant.LogoPath == null ? null : ContentAbsUrl(r.RestaurantChain.Restaurant.LogoPath),
                    FullName = r.RestaurantChain.FullName,
                    Email = r.RestaurantChain.Email,
                    Address1 = r.RestaurantChain.Address1,
                    Address2 = r.RestaurantChain.Address2,
                    Phone = r.RestaurantChain.Phone,
                    CityTown = r.RestaurantChain.City,
                    State = r.RestaurantChain.State,
                    Zip = r.RestaurantChain.Zip,
                    RestaurantNumber = r.RestaurantChain.RestaurantNumber,
                    TableLayoutPath = r.RestaurantChain.TableLayoutPath == null ? null : ContentAbsUrl(r.RestaurantChain.TableLayoutPath),
                    OpeningHour = r.RestaurantChain.OpeningHour,
                    ClosingHour = r.RestaurantChain.ClosingHour,
                    WaittimeInterval = r.RestaurantChain.WaittimeInterval,
                    RestaurantClosedDays = r.RestaurantChain.RestaurantClosedDays.Select(x => new
                    {
                        Day = x.Days
                    })
                };
                return CreateHttpResponse(jsonResult);
            }

            return new HttpResponseMessage(HttpStatusCode.NotFound);
        }

        /// <summary>
        /// Update Restaurant Info
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("api/restaurant")]
        [Authorize(Roles = "Administrator, Regional Manager")]
        [EnableCors(origins: "*", headers: "*", methods: "*", SupportsCredentials = true)]
        public HttpResponseMessage Put(RestaurantInfoModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.RestaurantId == null)
                {
                    var userId = _userRepository.GetUserId(Username);
                    model.UserId = userId;
                    var request = new AddAccountRequest(model);
                    Facade.Command(request);
                }
                else
                {
                    var request = new EditRestaurantChainRequest(model);
                    Facade.Command(request);
                }

                return CreateHttpResponse(new { });
            }

            throw new InvalidPostDataException(ModelState);

        }


        [Route("api/restaurant/upload")]
        [Authorize(Roles = "Administrator, Regional Manager")]
        [EnableCors(origins: "*", headers: "*", methods: "POST", SupportsCredentials = true)]
        public HttpResponseMessage Post(ImageModel model)
        {
            if (ModelState.IsValid)
            {

                if (model.Image != null && ImageHelper.FileIsWebFriendlyImage(model.InputStream))
                {
                    if (ImageHelper.ValidateImage(model.InputStream, Core.Helpers.ConfigurationHelper.Instance.UserProfileMaxResolution,
                                                                    Core.Helpers.ConfigurationHelper.Instance.UserProfileMinResolution))
                    {
                        string imagePath = string.Empty;
                        try
                        {
                            imagePath = ImageHelper.SaveImage(model.InputStream, Guid.NewGuid().ToString() + Core.Helpers.FileHelper.GetExtensionFromFileName(model.Image.FileName));

                            var restaurantId = 0;
                            RestaurantChainIdModel m = new RestaurantChainIdModel();
                            m.RestaurantChainId = model.RestaurantChainId;
                            var res = _accountInfoRepository.GetRestaurantInfo(m);
                            restaurantId = (int)res.RestaurantId;

                            var r = _accountInfoRepository.UpdateImage(restaurantId, imagePath);
                            return CreateHttpResponse(new { ImagePath = Url.Content(imagePath) });
                        }
                        catch (Exception ex)
                        {
                            ImageHelper.DeleteImageWithRelativePath(imagePath);
                            ModelState.AddModelError("Validation", ex.Message);
                            throw new InvalidPostDataException(ModelState);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("photo", String.Format("Image must between {0}X{1} to {2}X{3}.",
                               ConfigurationHelper.Instance.UserProfileMinResolution.Width,
                               ConfigurationHelper.Instance.UserProfileMinResolution.Height,
                               ConfigurationHelper.Instance.UserProfileMaxResolution.Width,
                               ConfigurationHelper.Instance.UserProfileMaxResolution.Height));
                        throw new InvalidPostDataException(ModelState);
                    }
                }
                else
                {
                    ModelState.AddModelError("photo", "Not valid file");
                }
            }

            throw new InvalidPostDataException(ModelState);

        }


        [Route("api/restaurant/uploadtablelayout")]
        [Authorize(Roles = "Administrator, Regional Manager")]
        [EnableCors(origins: "*", headers: "*", methods: "POST", SupportsCredentials = true)]
        [HttpPost]
        public HttpResponseMessage UploadTableLayout(ImageModel model)
        {
            if (ModelState.IsValid)
            {

                if (model.Image != null && ImageHelper.FileIsWebFriendlyImage(model.InputStream))
                {
                    if (ImageHelper.ValidateImage(model.InputStream, Core.Helpers.ConfigurationHelper.Instance.UserProfileMaxResolution,
                                                                    Core.Helpers.ConfigurationHelper.Instance.UserProfileMinResolution))
                    {
                        string imagePath = string.Empty;
                        try
                        {
                            imagePath = ImageHelper.SaveImage(model.InputStream, Guid.NewGuid().ToString() + Core.Helpers.FileHelper.GetExtensionFromFileName(model.Image.FileName));

                            var r = _accountInfoRepository.UpdateTableLayout(model.RestaurantChainId, imagePath);
                            return CreateHttpResponse(new { ImagePath = Url.Content(imagePath) });
                        }
                        catch (Exception ex)
                        {
                            ImageHelper.DeleteImageWithRelativePath(imagePath);
                            ModelState.AddModelError("Validation", ex.Message);
                            throw new InvalidPostDataException(ModelState);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("photo", String.Format("Image must between {0}X{1} to {2}X{3}.",
                               ConfigurationHelper.Instance.UserProfileMinResolution.Width,
                               ConfigurationHelper.Instance.UserProfileMinResolution.Height,
                               ConfigurationHelper.Instance.UserProfileMaxResolution.Width,
                               ConfigurationHelper.Instance.UserProfileMaxResolution.Height));
                        throw new InvalidPostDataException(ModelState);
                    }
                }
                else
                {
                    ModelState.AddModelError("photo", "Not valid file");
                }
            }

            throw new InvalidPostDataException(ModelState);

        }
        public string ContentAbsUrl(string relativeContentPath)
        {
            Uri contextUri = HttpContext.Current.Request.Url;

            var baseUri = string.Format("{0}://{1}{2}", contextUri.Scheme,
               contextUri.Host, contextUri.Port == 80 ? string.Empty : ":" + contextUri.Port);
            if (relativeContentPath == null)
            {
                return baseUri;
            }
            return string.Format("{0}{1}", baseUri, VirtualPathUtility.ToAbsolute(relativeContentPath));
        }

        /// <summary>
        /// Get Restaurant List
        /// </summary>
        /// <param name="restaurantid"></param>
        /// <returns></returns>
        [Route("api/restaurant/restaurantlist")]
        [Authorize(Roles = "Administrator,Regional Manager")]
        [EnableCors(origins: "*", headers: "*", methods: "*", SupportsCredentials = true)]
        [HttpGet]
        public HttpResponseMessage RestaurantList()
        {
            if (Administrator)
            {
                var request = new GetAllRestaurantRequest();
                var response = Facade.Query<GetAllRestaurantResponse, GetAllRestaurantRequest>(request);
                var jsonResult = response.RestaurantChains.Select(x => new
                {
                    RestaurantChainId = x.RestaurantChainId,
                    BusinessName = x.Restaurant.BusinessName

                });
                return CreateHttpResponse(jsonResult);
            }
            else
            {
                //var request = new GetRestaurantChainsRequest((int)restaurantId);
                //var response = Facade.Query<GetRestaurantChainsResponse, GetRestaurantChainsRequest>(request);
                var userId = _userRepository.GetUserId(Username);
                var request = new GetUserRestaurantsRequest(userId);
                var response = Facade.Query<GetUserRestaurantsResponse, GetUserRestaurantsRequest>(request);

                var jsonResult = response.UsersInRestaurant.Select(x => new
                {
                    RestaurantChainId = x.RestaurantChainId,
                    BusinessName = x.RestaurantChain.Restaurant.BusinessName

                });
                return CreateHttpResponse(jsonResult);
            }
        }

        /// <summary>
        /// Get Restaurant List
        /// </summary>
        /// <param name="restaurantid"></param>
        /// <returns></returns>
        [Route("api/restaurant/restaurantchains")]
        [Authorize(Roles = "Administrator,Regional Manager")]
        [EnableCors(origins: "*", headers: "*", methods: "*", SupportsCredentials = true)]
        [HttpGet]
        public HttpResponseMessage RestaurantChains(int RestaurantId)
        {

            var request = new GetRestaurantChainsRequest((int)RestaurantId);
            var response = Facade.Query<GetRestaurantChainsResponse, GetRestaurantChainsRequest>(request);
            var jsonResult = response.RestaurantChain.Select(x => new
            {
                RestaurantChainId = x.RestaurantChainId,
                BusinessName = x.Restaurant.BusinessName

            });
            return CreateHttpResponse(jsonResult);
        }

        [Route("api/restaurant/addbusiness")]
        [Authorize(Roles = "Administrator,Regional Manager")]
        [EnableCors(origins: "*", headers: "*", methods: "*", SupportsCredentials = true)]
        public HttpResponseMessage Post(RestaurantBusinessModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var usr = _userRepository.GetUserId(Username);
                    model.UserId = usr;
                    var command = new AddRestaurantBusinessRequest(model);
                    Facade.Command(command);

                    var request = new GetRestaurantBusinessRequest((int)model.BusinessId, usr);
                    var response = Facade.Query<GetRestaurantBusinessResponse, GetRestaurantBusinessRequest>(request);
                    var jsonResult = new
                    {
                        RestaurantChainId = response.UsersInRestaurant.RestaurantChainId

                    };
                    return CreateHttpResponse(jsonResult);
                }
                catch (ApiException exception)
                {
                    return CreateHttpResponse(exception);
                }
            }
            throw new InvalidPostDataException(ModelState);
        }

        [AllowAnonymous]
        [Route("api/restaurant/GetRestaurantByBusinessId")]
        [HttpGet]
        [EnableCors(origins: "*", headers: "*", methods: "*", SupportsCredentials = true)]
        public HttpResponseMessage GetRestaurantByBusinessId(int BusinessId)
        {

            var request = new GetRestaurantByBusinessIdRequest(BusinessId);
            var r = Facade.Query<GetRestaurantByBusinessIdResponse, GetRestaurantByBusinessIdRequest>(request);
            if (r != null)
            {
                int averageWaitTime = _waitListRepository.GetAverageWaitTime((int)r.Restaurant.RestaurantChainId);
                var requestGuestType = new GetGuestTypeRequest();
                var resultGuestType = Facade.Query<GetGuestTypeResponse, GetGuestTypeRequest>(requestGuestType);

                var gt = resultGuestType.GuestType;

                var jsonResult = new
                {
                    RestaurantId = r.Restaurant.RestaurantChain.RestaurantId,
                    RestaurantChainId = r.Restaurant.RestaurantChainId,
                    BusinessName = r.Restaurant.RestaurantChain.Restaurant.BusinessName,
                    LogoPath = r.Restaurant.RestaurantChain.Restaurant.LogoPath == null ? null : ContentAbsUrl(r.Restaurant.RestaurantChain.Restaurant.LogoPath),
                    Address1 = r.Restaurant.RestaurantChain.Address1,
                    Address2 = r.Restaurant.RestaurantChain.Address2,
                    Phone = r.Restaurant.RestaurantChain.Phone,
                    CityTown = r.Restaurant.RestaurantChain.City,
                    State = r.Restaurant.RestaurantChain.State,
                    Zip = r.Restaurant.RestaurantChain.Zip,
                    AverageWaitTime = averageWaitTime,
                    GuestType = gt.Select(g => new
                    {
                        GuestTypeId = g.GuestTypeId,
                        GuestType = g.GuestType1

                    }),
                };
                return CreateHttpResponse(jsonResult);
            }

            return new HttpResponseMessage(HttpStatusCode.NotFound);
        }

        [Route("api/restaurant/waitlist/")]
        [AllowAnonymous]
        [EnableCors(origins: "*", headers: "*", methods: "*", SupportsCredentials = true)]
        public HttpResponseMessage Post(InsertWaitList model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var userId = _userRepository.GetUserId(Username);

                    var request = new AddWaitListRequest(model, userId);
                    Facade.Command(request);

                    return CreateHttpResponse(HttpStatusCode.OK);
                }
                catch (ApiException exception)
                {
                    return CreateHttpResponse(exception);
                }
            }
            throw new InvalidPostDataException(ModelState);
        }

        /// <summary>
        /// Get Restaurant Tables
        /// </summary>
        /// <param name="restaurantchainid"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/tables/")]
        [Authorize(Roles = "Administrator,Regional Manager")]
        [EnableCors(origins: "*", headers: "*", methods: "*", SupportsCredentials = true)]
        public HttpResponseMessage GetRestaurantTables(int RestaurantChainId)
        {
            if (ModelState.IsValid)
            {
                var tables = _accountInfoRepository.GetRestaurantTables(RestaurantChainId);
                var tableTypes = _accountInfoRepository.GetTableType();

                var response = new
                {
                    Businesses = tables.Select(u => new
                    {
                        TableId = u.TableId,
                        TableNumber = u.TableNumber,
                        TableTypeId = u.TableTypeId,
                        Title = u.TableType.Title,
                        MaxSeating = u.TableType.MaxSeating,
                        IsAvailable = u.IsAvailable ?? true
                    }),
                    Tables = tableTypes.Select(x => new
                    {
                        TableTypeId = x.TableTypeId,
                        Title = x.Title,
                        MaxSeating = x.MaxSeating
                    })
                };
                return CreateHttpResponse(response);
            }
            throw new InvalidPostDataException(ModelState);
        }


        [HttpGet]
        [Route("api/tables/")]
        [Authorize(Roles = "Administrator,Regional Manager")]
        [EnableCors(origins: "*", headers: "*", methods: "*", SupportsCredentials = true)]
        public HttpResponseMessage GetRestaurantTableInfo(int TableId)
        {
            if (ModelState.IsValid)
            {
                var u = _accountInfoRepository.GetRestaurantTableInfo(TableId);
                var tableTypes = _accountInfoRepository.GetTableType();

                var response = new
                {
                    TableId = u.TableId,
                    TableNumber = u.TableNumber,
                    TableTypeId = u.TableTypeId,
                    Title = u.TableType.Title,
                    MaxSeating = u.TableType.MaxSeating,
                    IsAvailable = u.IsAvailable ?? true
                };

                return CreateHttpResponse(response);
            }
            throw new InvalidPostDataException(ModelState);
        }

        [HttpPost]
        [Route("api/tables/")]
        [AllowAnonymous]
        [EnableCors(origins: "*", headers: "*", methods: "*", SupportsCredentials = true)]
        public HttpResponseMessage AddTable(RestaurantTableModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _accountInfoRepository.AddRestaurantTable(model);

                    return CreateHttpResponse(HttpStatusCode.OK);
                }
                catch (ApiException exception)
                {
                    return CreateHttpResponse(exception);
                }
            }
            throw new InvalidPostDataException(ModelState);
        }

        [HttpPut]
        [Route("api/tables/")]
        [AllowAnonymous]
        [EnableCors(origins: "*", headers: "*", methods: "*", SupportsCredentials = true)]
        public HttpResponseMessage EditTable(RestaurantTableModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _accountInfoRepository.UpdateRestaurantTable(model);

                    return CreateHttpResponse(HttpStatusCode.OK);
                }
                catch (ApiException exception)
                {
                    return CreateHttpResponse(exception);
                }
            }
            throw new InvalidPostDataException(ModelState);
        }

        [HttpPost]
        [Route("api/restaurant/assigntable/")]
        [Authorize(Roles = "Administrator,Regional Manager")]
        [EnableCors(origins: "*", headers: "*", methods: "*", SupportsCredentials = true)]
        public HttpResponseMessage AssignTable(AssignRestaurantTableModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var request = new AssignStaffRequest(model);
                    Facade.Command(request);
                    return CreateHttpResponse(HttpStatusCode.OK);
                }
                catch (ApiException exception)
                {
                    return CreateHttpResponse(exception);
                }
            }
            throw new InvalidPostDataException(ModelState);
        }
    }
}
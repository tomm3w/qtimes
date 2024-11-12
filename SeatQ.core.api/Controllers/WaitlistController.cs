using common.api;
using common.api.Infrastructure;
using common.api.Infrastructure.Exeptions;
using Core.Extensions;
using SeatQ.core.api.Commands.AddWaitList;
using SeatQ.core.api.Commands.EditWaitList;
using SeatQ.core.api.Commands.UpdateConfirmed;
using SeatQ.core.api.Commands.UpdateLeave;
using SeatQ.core.api.Commands.UpdateLeft;
using SeatQ.core.api.Commands.UpdateSeated;
using SeatQ.core.api.Exceptions;
using SeatQ.core.api.Queries.GetGuestById;
using SeatQ.core.api.Queries.GetGuestMessages;
using SeatQ.core.api.Queries.GetGuestType;
using SeatQ.core.api.Queries.GetLoyaltyMessages;
using SeatQ.core.api.Queries.GetRestaurant;
using SeatQ.core.api.Queries.GetRestaurantChain;
using SeatQ.core.api.Queries.GetUserRestaurants;
using SeatQ.core.api.Queries.GetWaitListById;
using SeatQ.core.dal.Infrastructure.Repositories;
using SeatQ.core.dal.Models;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Configuration;
using System.Web.Http;
using System.Web.Http.Cors;
using userinfo.dal.Repositories;

namespace SeatQ.core.api.Controllers
{
    [Filters.InvalidPostDataExceptionFilter]
    public class WaitlistController : SpecificApiController
    {
        private readonly IWaitListRepository _waitListRepository;
        private readonly IMessageRepository _messageRepository;
        private readonly IUserRepository _userRepository;
        private readonly IAccountInfoRepository _accountInfoRepository;
        public WaitlistController(IApplicationFacade applicationFacade, IWaitListRepository waitListRepository, IUserRepository userRepository, IMessageRepository messageRepository, IAccountInfoRepository accountInfoRepository)
            : base(applicationFacade)
        {
            _waitListRepository = waitListRepository;
            _userRepository = userRepository;
            _messageRepository = messageRepository;
            _accountInfoRepository = accountInfoRepository;
        }

        [Route("api/waitlist/")]
        [Authorize(Roles = "Administrator,Regional Manager, User")]
        [EnableCors(origins: "*", headers: "*", methods: "*", SupportsCredentials = true)]
        public HttpResponseMessage Get([FromUri]PagingModel model, int RestaurantChainId)
        {
            if (ModelState.IsValid)
            {
                //if rcid=0, get the top 1 rcid
                //else procees with requested rcid
                var userId = _userRepository.GetUserId(Username);
                if (RestaurantChainId == 0)
                {
                    //get the top 1 rcid
                    var requestUsersInRestaurants = new GetUserRestaurantsRequest(userId);
                    var responseUsersInRestaurants = Facade.Query<GetUserRestaurantsResponse, GetUserRestaurantsRequest>(requestUsersInRestaurants);
                    if (responseUsersInRestaurants != null && responseUsersInRestaurants.UsersInRestaurant.Count > 0)
                    {
                        RestaurantChainId = (int)responseUsersInRestaurants.UsersInRestaurant.FirstOrDefault().RestaurantChainId;
                    }
                }


                var wl = _waitListRepository.GetWaitList(model, RestaurantChainId);

                var requestGuestType = new GetGuestTypeRequest();
                var resultGuestType = Facade.Query<GetGuestTypeResponse, GetGuestTypeRequest>(requestGuestType);

                var gt = resultGuestType.GuestType;

                int averageWaitTime = _waitListRepository.GetAverageWaitTime((int)RestaurantChainId);

                RestaurantChainIdModel rc = new RestaurantChainIdModel();
                rc.RestaurantChainId = RestaurantChainId;
                var restaurant = _accountInfoRepository.GetRestaurantInfo(rc);
                var businessJson = new
                {
                    RestaurantChainId = RestaurantChainId,
                    RestaurantNumber = restaurant == null ? "" : restaurant.RestaurantNumber,
                    PagingData = wl.Paging,
                    BusinessName = restaurant == null ? "" : restaurant.Restaurant.BusinessName,
                    LogoPath = restaurant == null ? null : (restaurant.Restaurant.LogoPath == null ? null : ContentAbsUrl(restaurant.Restaurant.LogoPath)),
                    Businesses = wl.Businesses.Select(w => new
                    {
                        WaitListId = w.WaitListId,
                        GuestName = w.GuestName,
                        MobileNumber = w.MobileNumber,
                        GroupSize = w.GroupSize,
                        ActualDateTime = w.ActualDateTime.ToUniversalTimeString(),
                        EstimatedDateTime = w.EstimatedDateTime.ToUniversalTimeString(),
                        SeatedDateTime = w.SeatedDateTime.ToUniversalTimeString(),
                        MessageReply = w.MessageReply,
                        IsSeated = w.IsSeated,
                        NoOfReturn = w.NoOfReturn,
                        Visit = w.Visit,
                        GuestTypeId = w.GuestTypeId,
                        IsMessageSent = w.IsMessageSent,
                        TableNumber = w.TableNumber,
                        RoomNumber = w.RoomNumber,
                        GuestMessageCount = w.GuestMessageCount,
                        AvgTableTime = w.AvgTableTime,
                        IsLeft = w.IsLeft,
                        LeftDateTime = w.LeftDateTime,
                        Comments = w.Comments,
                        RemainingTime = w.RemainingTime,
                        TotalMinuteSat = w.TotalMinuteSat,
                        LateSeating = w.LateSeating,
                        UnReadMessageCount = w.UnReadMessageCount
                    }),
                    GuestType = gt.Select(g => new
                    {
                        GuestTypeId = g.GuestTypeId,
                        GuestType = g.GuestType1

                    }),
                    Tables = restaurant.RestaurantTables.Where(x => (x.IsAvailable ?? true) == true).Select(t => new
                    {
                        TableId = t.TableId,
                        TableNumber = t.TableNumber,
                        Title = t.TableType.Title
                    }),
                    AverageWaitTime = averageWaitTime
                };

                return CreateHttpResponse(new { businesses = businessJson });
            }
            throw new InvalidPostDataException(ModelState);
        }

        [Route("api/waitlist/")]
        [Authorize(Roles = "Administrator,Regional Manager, User")]
        [EnableCors(origins: "*", headers: "*", methods: "*", SupportsCredentials = true)]
        public HttpResponseMessage Get(int WaitListId)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var request = new GetWaitListByIdRequest(WaitListId);
                    var wl = Facade.Query<GetWaitListByIdResponse, GetWaitListByIdRequest>(request);

                    var w = wl.WaitList;
                    var jsonResult = new
                    {
                        MyWaitListId = w.WaitListId,
                        MyGuestName = w.GuestName,
                        MyMobileNumber = w.GuestInfo.MobileNumber,
                        MyGroupSize = w.GroupSize,
                        MyActualDateTime = w.ActualDateTime.ToUniversalTimeString(),
                        MyEstimatedDateTime = w.EstimatedDateTime.ToUniversalTimeString(),
                        MyEnteredDateTime = w.EnteredDateTime.ToUniversalTimeString(),
                        MyGuestTypeId = w.GuestTypeId,
                        MyMessageReply = w.MessageReply,
                        MyIsSeated = w.IsSeated,
                        MyIsMessageSent = w.IsMessageSent,
                        MyTableId = w.TableId,
                        MyTableNumber = w.RestaurantTable == null ? null : w.RestaurantTable.TableNumber,
                        MyRoomNumber = w.RoomNumber,
                        MyIsLeft = w.IsLeft,
                        MyComments = w.Comments
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

        [Route("api/waitlist/Message")]
        [Authorize(Roles = "Administrator,Regional Manager, User")]
        [EnableCors(origins: "*", headers: "*", methods: "*", SupportsCredentials = true)]
        [HttpGet]
        public HttpResponseMessage Message(int WaitListId)
        {
            if (ModelState.IsValid)
            {
                try
                {

                    var requestWaitlist = new GetWaitListByIdRequest(WaitListId);
                    var wlist = Facade.Query<GetWaitListByIdResponse, GetWaitListByIdRequest>(requestWaitlist);

                    var wait = wlist.WaitList;

                    var request = new GetGuestMessageByIdRequest(WaitListId);
                    var wl = Facade.Query<GetGuestMessageByIdResponse, GetGuestMessageByIdRequest>(request);

                    var msg = wl.GuestMessage;

                    var jsonResult = new
                    {
                        count = msg.Count,
                        GuestName = wait.GuestName,
                        GuestMessage = msg.Select(w => new
                        {
                            WaitListId = w.WaitListId,
                            GuestMessageId = w.GuestMessageId,
                            Message = w.Message,
                            MessageDateTime = w.MessageDateTime.ToUniversalTimeString(),
                            MessageFrom = w.MessageFrom,
                            GuestName = w.WaitList.GuestName
                        })
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

        [Route("api/waitlist/")]
        [Authorize(Roles = "Administrator,Regional Manager, User")]
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

        [Route("api/waitlist/")]
        [Authorize(Roles = "Administrator,Regional Manager, User")]
        [EnableCors(origins: "*", headers: "*", methods: "*", SupportsCredentials = true)]
        public HttpResponseMessage Put(InsertWaitList model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var userId = _userRepository.GetUserId(Username);
                    var req = new EditWaitListRequest(model, userId);
                    Facade.Command(req);

                    var request = new GetWaitListByIdRequest(model.WaitListId);
                    var response = Facade.Query<GetWaitListByIdResponse, GetWaitListByIdRequest>(request);
                    var jsonResult = new
                    {
                        WaitListId = response.WaitList.WaitListId,
                        GuestName = response.WaitList.GuestName,
                        MobileNumber = response.WaitList.GuestInfo.MobileNumber,
                        GroupSize = response.WaitList.GroupSize,
                        EstimatedDateTime = response.WaitList.EstimatedDateTime,
                        RestaurantChainId = response.WaitList.RestaurantChainId,
                        EnteredBy = response.WaitList.EnteredBy
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


        [Route("api/waitlist/SendTextMessage")]
        [Authorize(Roles = "Administrator,Regional Manager, User")]
        [EnableCors(origins: "*", headers: "*", methods: "*", SupportsCredentials = true)]
        public HttpResponseMessage Post(WaitListIdModel model)
        {

            string key = WebConfigurationManager.AppSettings["NexmoKey"];
            string secret = WebConfigurationManager.AppSettings["NexmoSecret"];

            if (ModelState.IsValid)
            {
                try
                {
                    var request = new GetWaitListByIdRequest(model.WaitListId);
                    var wl = Facade.Query<GetWaitListByIdResponse, GetWaitListByIdRequest>(request);

                    if (wl != null)
                    {
                        string From = wl.WaitList.RestaurantChain.RestaurantNumber;
                        if (string.IsNullOrEmpty(From))
                        {

                            var jsonResult = new
                            {
                                Message = "Restaurant number not assigned."
                            };
                            return CreateHttpResponse(jsonResult);
                        }

                        string To = wl.WaitList.GuestInfo.MobileNumber;
                        string msg = _messageRepository.GetReadyMessage((int)wl.WaitList.RestaurantChainId);
                        msg = msg + "\nPlease REPLY \"1\" FOR CONFIRMED OR \"2\" FOR CANCEL";

                        if (string.IsNullOrEmpty(msg))
                        {
                            var jsonResult = new
                            {
                                Message = "No ready message defined."
                            };
                            return CreateHttpResponse(jsonResult);
                        }


                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                        var response = new HastyAPI.Nexmo.Nexmo(key, secret).Send(From, To, msg);

                        if (response.MessageCount > 0)
                        {

                            if (response.Messages[0].Status == 0)
                            {
                                _waitListRepository.SendTextMessage((int)model.WaitListId);
                            }

                            _messageRepository.SaveResponse(wl.WaitList, response, From, msg);

                            return CreateHttpResponse(response.Messages);

                        }
                    }

                }
                catch (ApiException exception)
                {
                    return CreateHttpResponse(exception);
                }
            }
            throw new InvalidPostDataException(ModelState);
        }

        [Route("api/waitlist/Confirmed")]
        [Authorize(Roles = "Administrator,Regional Manager, User")]
        [EnableCors(origins: "*", headers: "*", methods: "*", SupportsCredentials = true)]
        [HttpPut]
        public HttpResponseMessage Confirmed(WaitListIdModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var request = new UpdateConfirmedRequest(model);
                    Facade.Command(request);

                    var req = new GetWaitListByIdRequest(request.Model.WaitListId);
                    var response = Facade.Query<GetWaitListByIdResponse, GetWaitListByIdRequest>(req);

                    var jsonResult = new
                    {
                        WaitListId = response.WaitList.WaitListId,
                        MessageSentDateTime = response.WaitList.MessageReplyRecivedDatetime.ToUniversalTimeString()
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

        [Route("api/waitlist/Leave")]
        [Authorize(Roles = "Administrator,Regional Manager, User")]
        [EnableCors(origins: "*", headers: "*", methods: "*", SupportsCredentials = true)]
        [HttpPut]
        public HttpResponseMessage Leave(WaitListIdModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var request = new UpdateLeaveRequest(model);
                    Facade.Command(request);

                    var req = new GetWaitListByIdRequest(request.Model.WaitListId);
                    var response = Facade.Query<GetWaitListByIdResponse, GetWaitListByIdRequest>(req);

                    var jsonResult = new
                    {
                        WaitListId = response.WaitList.WaitListId,
                        MessageSentDateTime = response.WaitList.MessageReplyRecivedDatetime.ToUniversalTimeString()
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

        [Route("api/waitlist/Seated")]
        [Authorize(Roles = "Administrator,Regional Manager, User")]
        [EnableCors(origins: "*", headers: "*", methods: "*", SupportsCredentials = true)]
        [HttpPut]
        public HttpResponseMessage Seated(WaitListSeatedModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var request = new UpdateSeatedRequest(model);
                    Facade.Command(request);

                    var req = new GetWaitListByIdRequest(request.Model.WaitListId);
                    var response = Facade.Query<GetWaitListByIdResponse, GetWaitListByIdRequest>(req);

                    var jsonResult = new
                    {
                        WaitListId = response.WaitList.WaitListId,
                        SeatedDateTime = response.WaitList.SeatedDateTime.ToUniversalTimeString()
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


        [Route("api/waitlist/Left")]
        [Authorize(Roles = "Administrator,Regional Manager, User")]
        [EnableCors(origins: "*", headers: "*", methods: "*", SupportsCredentials = true)]
        [HttpPut]
        public HttpResponseMessage Left(WaitListIdModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var request = new UpdateLeftRequest(model);
                    Facade.Command(request);

                    var req = new GetWaitListByIdRequest(request.Model.WaitListId);
                    var response = Facade.Query<GetWaitListByIdResponse, GetWaitListByIdRequest>(req);

                    var jsonResult = new
                    {
                        WaitListId = response.WaitList.WaitListId,
                        LeftDateTime = response.WaitList.LeftDateTime.ToUniversalTimeString()
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


        [Route("api/waitlist/SendLeftMessage")]
        [Authorize(Roles = "Administrator,Regional Manager, User")]
        [EnableCors(origins: "*", headers: "*", methods: "*", SupportsCredentials = true)]
        public HttpResponseMessage SendLeftMessage(WaitListIdModel model)
        {

            string key = WebConfigurationManager.AppSettings["NexmoKey"];
            string secret = WebConfigurationManager.AppSettings["NexmoSecret"];

            if (ModelState.IsValid)
            {
                try
                {
                    var request = new GetWaitListByIdRequest(model.WaitListId);
                    var wl = Facade.Query<GetWaitListByIdResponse, GetWaitListByIdRequest>(request);

                    if (wl != null)
                    {
                        string From = wl.WaitList.RestaurantChain.RestaurantNumber;
                        if (string.IsNullOrEmpty(From))
                        {

                            var jsonResult = new
                            {
                                Message = "Restaurant number not assigned."
                            };
                            return CreateHttpResponse(jsonResult);
                        }

                        string To = wl.WaitList.GuestInfo.MobileNumber;
                        string msg = _messageRepository.GetVisitMessage((int)wl.WaitList.RestaurantChainId, (int)wl.WaitList.Visit);

                        if (string.IsNullOrEmpty(msg))
                        {
                            var jsonResult = new
                            {
                                Message = "No visit notification defined."
                            };
                            return CreateHttpResponse(jsonResult);
                        }

                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                        var response = new HastyAPI.Nexmo.Nexmo(key, secret).Send(From, To, msg);

                        if (response.MessageCount > 0)
                        {

                            _waitListRepository.SaveVisitMessage(wl.WaitList, response, From, To, msg);

                            return CreateHttpResponse(response.Messages);

                        }
                    }

                }
                catch (ApiException exception)
                {
                    return CreateHttpResponse(exception);
                }
            }
            throw new InvalidPostDataException(ModelState);
        }

        //DONE
        [Route("api/waitlist/ReturnGuest")]
        [Authorize(Roles = "Administrator,Regional Manager")]
        [EnableCors(origins: "*", headers: "*", methods: "*", SupportsCredentials = true)]
        [HttpGet]
        public HttpResponseMessage Get([FromUri]PagingModel model, [FromUri]MetricsModel where)
        {
            if (ModelState.IsValid)
            {

                var w = _waitListRepository.GetReturnGuest(model, where);
                var businessJson = new
                {
                    PagingData = w.Paging,
                    Businesses = w.Businesses.Select(x => new
                    {
                        RestaurantChainId = x.RestaurantChainId,
                        GuestId = x.GuestId,
                        GuestName = x.GuestName,
                        MobileNumber = x.MobileNumber,
                        NoOfReturn = x.NoOfReturn
                    })
                };

                return CreateHttpResponse(new { businesses = businessJson });
            }

            throw new InvalidPostDataException(ModelState);

        }

        //DONE
        [Route("api/waitlist/loyalty")]
        [Authorize(Roles = "Administrator,Regional Manager")]
        [EnableCors(origins: "*", headers: "*", methods: "*", SupportsCredentials = true)]
        [HttpGet]
        public HttpResponseMessage loyalty([FromUri]PagingModel model, [FromUri]int RestaurantChainId)
        {
            if (ModelState.IsValid)
            {

                var w = _waitListRepository.GetLoyalty(model, RestaurantChainId);
                var businessJson = new
                {
                    PagingData = w.Paging,
                    Businesses = w.Businesses.Select(x => new
                    {
                        GuestId = x.GuestId,
                        GuestName = x.GuestName,
                        MobileNumber = x.MobileNumber,
                        NoOfReturn = x.NoOfReturn,
                        LastVisit = x.LastVisit,
                        Total = x.Total,
                        Week = x.Week,
                        Month = x.Month,
                        Year = x.Year,
                        UnReadMessageCount = x.UnReadMessageCount
                    })
                };

                return CreateHttpResponse(new { businesses = businessJson });
            }

            throw new InvalidPostDataException(ModelState);

        }

        [Route("api/waitlist/SendMessage")]
        [Authorize(Roles = "Administrator,Regional Manager, User")]
        [EnableCors(origins: "*", headers: "*", methods: "*", SupportsCredentials = true)]
        public HttpResponseMessage Post(GetReturnGuest_Result model)
        {
            if (ModelState.IsValid)
            {
                string key = WebConfigurationManager.AppSettings["NexmoKey"];
                string secret = WebConfigurationManager.AppSettings["NexmoSecret"];

                var request = new GetRestaurantChainRequest((int)model.RestaurantChainId);
                var wl = Facade.Query<GetRestaurantChainResponse, GetRestaurantChainRequest>(request);

                string From = wl.RestaurantChain.RestaurantNumber;
                string To = model.MobileNumber;
                string msg = null;
                try
                {
                    msg = _messageRepository.GetVisitMessage(model);
                }
                catch (Exception ex)
                {
                    var jsonResult = new
                    {
                        Message = ex.InnerException.Message
                    };
                    return CreateHttpResponse(jsonResult);
                }

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                var response = new HastyAPI.Nexmo.Nexmo(key, secret).Send(From, To, msg);

                if (response.MessageCount > 0)
                {
                    _messageRepository.SaveVisitResponse(model, response, From, msg);

                    return CreateHttpResponse(response.Messages);

                }
            }

            throw new InvalidPostDataException(ModelState);
        }


        [Route("api/waitlist/SendMessageToGuest")]
        [Authorize(Roles = "Administrator,Regional Manager, User")]
        [EnableCors(origins: "*", headers: "*", methods: "*", SupportsCredentials = true)]
        public HttpResponseMessage Post(SendMessageToGuestModel model)
        {

            string key = WebConfigurationManager.AppSettings["NexmoKey"];
            string secret = WebConfigurationManager.AppSettings["NexmoSecret"];

            if (ModelState.IsValid)
            {
                try
                {
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                    var request = new GetWaitListByIdRequest(model.WaitListId);
                    var wl = Facade.Query<GetWaitListByIdResponse, GetWaitListByIdRequest>(request);

                    if (wl != null)
                    {
                        string From = wl.WaitList.RestaurantChain.RestaurantNumber;
                        if (string.IsNullOrEmpty(From))
                        {

                            var jsonResult = new
                            {
                                Message = "Restaurant number not assigned."
                            };
                            return CreateHttpResponse(jsonResult);
                        }

                        string To = wl.WaitList.GuestInfo.MobileNumber;
                        string msg = model.Message;

                        if (string.IsNullOrEmpty(msg))
                        {
                            var jsonResult = new
                            {
                                Message = "No reply message defined."
                            };
                            return CreateHttpResponse(jsonResult);
                        }

                        var response = new HastyAPI.Nexmo.Nexmo(key, secret).Send(From, To, msg);

                        if (response.MessageCount > 0)
                        {

                            if (response.Messages[0].Status == 0)
                            {
                                _waitListRepository.SendMessageToGuest(model, response, From, To, msg);
                            }

                            return CreateHttpResponse(response.Messages);

                        }
                    }

                }
                catch (ApiException exception)
                {
                    return CreateHttpResponse(exception);
                }
            }
            throw new InvalidPostDataException(ModelState);
        }

        [Route("api/waitlist/GuestType")]
        [Authorize(Roles = "Administrator,Regional Manager, User")]
        [EnableCors(origins: "*", headers: "*", methods: "*", SupportsCredentials = true)]
        public HttpResponseMessage Get()
        {
            var requestGuestType = new GetGuestTypeRequest();
            var resultGuestType = Facade.Query<GetGuestTypeResponse, GetGuestTypeRequest>(requestGuestType);

            var gt = resultGuestType.GuestType;
            var jsonResult = gt.Select(g => new
            {
                GuestTypeId = g.GuestTypeId,
                GuestType = g.GuestType1

            });
            return new HttpResponseMessage(HttpStatusCode.OK) { Content = new JsonContent(jsonResult) };
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

        [Route("api/waitlist/GetTableSummary")]
        [Authorize(Roles = "Administrator,Regional Manager, User")]
        [EnableCors(origins: "*", headers: "*", methods: "*", SupportsCredentials = true)]
        [HttpGet]
        public HttpResponseMessage GetTableSummary(int RestaurantChainId)
        {
            if (ModelState.IsValid)
            {

                var w = _waitListRepository.GetTableSummary(RestaurantChainId);
                var jsonResult = new
                {
                    Waiting = w.Waiting,
                    Seated = w.Seated,
                    C1_4Inline = w.C1_4Inline,
                    C5_6Inline = w.C5_6Inline,
                    C7_8Inline = w.C7_8Inline,
                    C9PlusInline = w.C9PlusInline,
                    C1_4Turnline = w.C1_4Turnline,
                    C5_6Turnline = w.C5_6Turnline,
                    C7_8Turnline = w.C7_8Turnline,
                    C9PlusTurnline = w.C9PlusTurnline
                };

                return CreateHttpResponse(jsonResult);
            }

            throw new InvalidPostDataException(ModelState);

        }

        #region Loyalty

        [Route("api/waitlist/LoyaltyMessage")]
        [Authorize(Roles = "Administrator,Regional Manager, User")]
        [EnableCors(origins: "*", headers: "*", methods: "*", SupportsCredentials = true)]
        [HttpGet]
        public HttpResponseMessage LoyaltyMessage(int GuestId)
        {
            if (ModelState.IsValid)
            {
                try
                {

                    var requesGuest = new GetGuestByIdRequest(GuestId);
                    var wlist = Facade.Query<GetGuestByIdResponse, GetGuestByIdRequest>(requesGuest);

                    var guest = wlist.GuestInfo;

                    var request = new GetLoyaltyMessageByIdRequest(GuestId);
                    var wl = Facade.Query<GetLoyaltyMessageByIdResponse, GetLoyaltyMessageByIdRequest>(request);

                    var msg = wl.LoyaltyMessage;

                    var jsonResult = new
                    {
                        count = msg.Count,
                        GuestName = guest.GuestName,
                        GuestMessage = msg.Select(w => new
                        {
                            GuestId = w.GuestId,
                            LoyaltyMessageId = w.LoyaltyMessageId,
                            Message = w.Message,
                            MessageDateTime = w.MessageDateTime.ToUniversalTimeString(),
                            MessageFrom = w.MessageFrom,
                            GuestName = w.GuestInfo.GuestName
                        })
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

        [Route("api/waitlist/SendLoyaltyMessageToGuest")]
        [Authorize(Roles = "Administrator,Regional Manager, User")]
        [EnableCors(origins: "*", headers: "*", methods: "*", SupportsCredentials = true)]
        public HttpResponseMessage Post(SendLoyaltyMessageToGuestModel model)
        {

            string key = WebConfigurationManager.AppSettings["NexmoKey"];
            string secret = WebConfigurationManager.AppSettings["NexmoSecret"];

            if (ModelState.IsValid)
            {
                try
                {

                    var requesGuest = new GetGuestByIdRequest(model.GuestId);
                    var wlist = Facade.Query<GetGuestByIdResponse, GetGuestByIdRequest>(requesGuest);

                    var wl = wlist.GuestInfo;

                    if (wl != null)
                    {
                        string From = wl.RestaurantChain.RestaurantNumber;
                        if (string.IsNullOrEmpty(From))
                        {

                            var jsonResult = new
                            {
                                Message = "Restaurant number not assigned."
                            };
                            return CreateHttpResponse(jsonResult);
                        }

                        string To = wl.MobileNumber;
                        string msg = model.Message;

                        if (string.IsNullOrEmpty(msg))
                        {
                            var jsonResult = new
                            {
                                Message = "No reply message defined."
                            };
                            return CreateHttpResponse(jsonResult);
                        }

                        var response = new HastyAPI.Nexmo.Nexmo(key, secret).Send(From, To, msg);

                        if (response.MessageCount > 0)
                        {

                            if (response.Messages[0].Status == 0)
                            {
                                _waitListRepository.SendLoyaltyMessageToGuest(model, response, From, To, msg);
                            }

                            return CreateHttpResponse(response.Messages);

                        }
                    }

                }
                catch (ApiException exception)
                {
                    return CreateHttpResponse(exception);
                }
            }
            throw new InvalidPostDataException(ModelState);
        }

        #endregion

        [Route("api/waitlist/tables")]
        [Authorize(Roles = "Administrator,Regional Manager, User")]
        [EnableCors(origins: "*", headers: "*", methods: "*", SupportsCredentials = true)]
        [HttpGet]
        public HttpResponseMessage GetTablesWithSeating(int RestaurantChainId)
        {
            if (ModelState.IsValid)
            {
                var wl = _waitListRepository.GetTablesWithSeating(RestaurantChainId, Authentication.Web.Security.CurrentUserId.ToString());
                var tables = wl.Select(t => new
                {
                    TableId = t.TableId,
                    TableNumber = t.TableNumber,
                    TableStatus = t.TableStatus,
                    AvgTimeInMinute = t.AvgTimeInMinute,
                    MaxSeating = t.MaxSeating,
                    TableType = t.TableType,
                    AvgTime = t.AvgTime,
                    IsAvailable = true,
                    AssignedTo = t.AssignedTo,
                    UserName = t.UserName
                });

                return CreateHttpResponse(tables);
            }
            throw new InvalidPostDataException(ModelState);
        }

        [Route("api/waitlist/closetable")]
        [Authorize(Roles = "Administrator,Regional Manager, User")]
        [EnableCors(origins: "*", headers: "*", methods: "*", SupportsCredentials = true)]
        [HttpGet]
        public HttpResponseMessage CloseTable(int RestaurantChainId, int TableId)
        {
            if (ModelState.IsValid)
            {
                var wl = _waitListRepository.CloseTable(RestaurantChainId, TableId);

                return CreateHttpResponse(HttpStatusCode.OK);
            }
            throw new InvalidPostDataException(ModelState);
        }
    }
}
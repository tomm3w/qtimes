using SeatQ.core.dal.Models;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using SeatQ.core.dal.Infrastructure.Repositories;
using SeatQ.core.api.Infrastructure;
using System.Linq;
using Core.Extensions;
using System.Web.Configuration;
using iDestn.core.api.Models;
using HastyAPI.Nexmo;
using System;
using common.api.Infrastructure;

namespace SeatQ.core.api.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [Authorize(Roles = "Administrator, Regional Manager, User")]
    public class WaitlistController : ApiController
    {
        private readonly IWaitListRepository _waitListRepository;
        private readonly IAccountInfoRepository _accountRepository;
        private readonly IMessageRepository _messageRepository;
        public WaitlistController(IWaitListRepository waitListRepository, IMessageRepository messageRepository, IAccountInfoRepository accountRepository)
        {
            _waitListRepository = waitListRepository;
            _messageRepository = messageRepository;
            _accountRepository = accountRepository;
        }

        [Route("api/waitlist/")]
        public HttpResponseMessage Post([FromBody]AddWaitListModel model)
        {
            var wl = _waitListRepository.AddWaitList(model);
            var jsonResult = new
                {
                    WaitListId = wl.WaitListId,
                    GuestName = wl.GuestInfo.GuestName,
                    MobileNumber = wl.GuestInfo.MobileNumber,
                    GroupSize = wl.GroupSize,
                    EstimatedDateTime = wl.EstimatedDateTime,
                    RestaurantChainId = wl.RestaurantChainId,
                    EnteredBy = wl.EnteredBy
                };

            return new HttpResponseMessage(HttpStatusCode.OK) { Content = new JsonContent(jsonResult) };

        }

        [Route("api/waitlist/")]
        public HttpResponseMessage Put([FromBody]AddWaitListModel model)
        {
            var wl = _waitListRepository.UpdateWaitList(model);
            var jsonResult = new
            {
                WaitListId = wl.WaitListId,
                GuestName = wl.GuestInfo.GuestName,
                MobileNumber = wl.GuestInfo.MobileNumber,
                GroupSize = wl.GroupSize,
                EstimatedDateTime = wl.EstimatedDateTime,
                RestaurantChainId = wl.RestaurantChainId,
                EnteredBy = wl.EnteredBy
            };

            return new HttpResponseMessage(HttpStatusCode.OK) { Content = new JsonContent(jsonResult) };

        }

        [Route("api/waitlist/")]
        public HttpResponseMessage Get([FromBody]PagingModel model, int RestaurantChainId)
        {

            var wl = _waitListRepository.GetWaitList(model, RestaurantChainId);
            var gt = _waitListRepository.GetGuestType();
            int AverageWaitTime = _waitListRepository.GetAverageWaitTime(RestaurantChainId);

            var businessJson = new
            {
                PagingData = wl.Paging,
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
                    GuestTypeId = w.GuestTypeId,
                    IsMessageSent = w.IsMessageSent
                }),
                GuestType = gt.Select(g => new
                {
                    GuestTypeId = g.GuestTypeId,
                    GuestType = g.GuestType1

                }),
                AverageWaitTime = AverageWaitTime
            };

            return new HttpResponseMessage(HttpStatusCode.OK) { Content = new JsonContent(new { businesses = businessJson }) };

        }

        [Route("api/waitlist/GetWaitlistById")]
        [HttpGet]
        public HttpResponseMessage GetWaitlistById([FromUri]WaitListIdModel model)
        {

            var w = _waitListRepository.GetWaitlistById((int)model.WaitListId);

            var jsonResult = new
            {
                MyWaitListId = w.WaitListId,
                MyGuestName = w.GuestInfo.GuestName,
                MyMobileNumber = w.GuestInfo.MobileNumber,
                MyGroupSize = w.GroupSize,
                MyActualDateTime = w.ActualDateTime.ToUniversalTimeString(),
                MyEstimatedDateTime = w.EstimatedDateTime.ToUniversalTimeString(),
                MyEnteredDateTime = w.EnteredDateTime.ToUniversalTimeString(),
                MyGuestTypeId = w.GuestTypeId,
                MyMessageReply = w.MessageReply,
                MyIsSeated = w.IsSeated,
                MyIsMessageSent = w.IsMessageSent

            };

            return new HttpResponseMessage(HttpStatusCode.OK) { Content = new JsonContent(jsonResult) };

        }

        [Route("api/waitlist/SendTextMessage")]
        [HttpPost]
        public HttpResponseMessage SendTextMessage([FromUri]WaitListIdModel model)
        {

            string key = WebConfigurationManager.AppSettings["NexmoKey"];
            string secret = WebConfigurationManager.AppSettings["NexmoSecret"];
            var wl = _waitListRepository.GetWaitlistById((int)model.WaitListId);

            if (wl != null)
            {
                string From = wl.RestaurantChain.RestaurantNumber;
                if (string.IsNullOrEmpty(From))
                {
                    return new HttpResponseMessage(HttpStatusCode.NotFound)
                    {
                        Content = new JsonContent(
                            new ErrorModel
                            {
                                Message = "Restaurant number not assigned."
                            })
                    };
                }

                string To = wl.GuestInfo.MobileNumber;
                string msg = _messageRepository.GetReadyMessage((int)wl.RestaurantChainId);
                msg = msg + "\nPlease REPLY \"1\" FOR CONFIRMED OR \"2\" FOR CANCEL.";

                if (string.IsNullOrEmpty(msg))
                {
                    return new HttpResponseMessage(HttpStatusCode.NotFound)
                    {
                        Content = new JsonContent(
                            new ErrorModel
                            {
                                Message = "No ready message defined."
                            })
                    };
                }

                var response = new Nexmo(key, secret).Send(From, To, msg);

                if (response.MessageCount > 0)
                {

                    if (response.Messages[0].Status == 0)
                    {
                        _waitListRepository.SendTextMessage((int)model.WaitListId);
                    }

                    _messageRepository.SaveResponse(wl, response, From, msg);

                    return new HttpResponseMessage(HttpStatusCode.OK) { Content = new JsonContent(response.Messages) };

                }

            }

            return new HttpResponseMessage(HttpStatusCode.NotFound);
        }

        [Route("api/waitlist/Confirmed")]
        [HttpPut]
        public HttpResponseMessage Confirmed(WaitListIdModel model)
        {
            var w = _waitListRepository.Confirmed((int)model.WaitListId);
            var jsonResult = new
            {
                WaitListId = w.WaitListId,
                MessageSentDateTime = w.MessageReplyRecivedDatetime.ToUniversalTimeString()
            };

            return new HttpResponseMessage(HttpStatusCode.OK) { Content = new JsonContent(jsonResult) };
        }

        [Route("api/waitlist/Leave")]
        [HttpPut]
        public HttpResponseMessage Leave(WaitListIdModel model)
        {
            var w = _waitListRepository.Leave((int)model.WaitListId);
            var jsonResult = new
            {
                WaitListId = w.WaitListId,
                MessageReplyRecivedDatetime = w.MessageReplyRecivedDatetime.ToUniversalTimeString()
            };

            return new HttpResponseMessage(HttpStatusCode.OK) { Content = new JsonContent(jsonResult) };
        }

        [Route("api/waitlist/Seated")]
        [HttpPut]
        public HttpResponseMessage Seated(WaitListIdModel model)
        {
            var w = _waitListRepository.Seated((int)model.WaitListId);
            var jsonResult = new
            {
                WaitListId = w.WaitListId,
                SeatedDateTime = w.SeatedDateTime.ToUniversalTimeString()
            };

            return new HttpResponseMessage(HttpStatusCode.OK) { Content = new JsonContent(jsonResult) };
        }

        [Route("api/waitlist/ReturnGuest")]
        [HttpGet]
        public HttpResponseMessage ReturnGuest([FromBody]PagingModel model, [FromBody]MetricsModel where)
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

            return new HttpResponseMessage(HttpStatusCode.OK) { Content = new JsonContent(new { businesses = businessJson }) };
        }


        [Route("api/waitlist/SendMessage")]
        [HttpPost]
        public HttpResponseMessage SendMessage([FromBody]GetReturnGuest_Result model)
        {
            string key = WebConfigurationManager.AppSettings["NexmoKey"];
            string secret = WebConfigurationManager.AppSettings["NexmoSecret"];
            string From = _accountRepository.GetRestaurantNumber((int)model.RestaurantChainId);
            string To = model.MobileNumber;
            string msg;
            try
            {
                msg = _messageRepository.GetVisitMessage(model);
            }
            catch (Exception ex)
            {
                return new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new JsonContent(
                        new ErrorModel
                        {
                            Message = ex.InnerException.Message
                        })
                };
            }

            var response = new Nexmo(key, secret).Send(From, To, msg);

            if (response.MessageCount > 0)
            {
                _messageRepository.SaveVisitResponse(model, response, From, msg);

                return new HttpResponseMessage(HttpStatusCode.OK) { Content = new JsonContent(response.Messages) };

            }

            return new HttpResponseMessage(HttpStatusCode.NotFound);

        }

        [Route("api/waitlist/GuestType")]
        [HttpGet]
        public HttpResponseMessage GuestType([FromBody]GetReturnGuest_Result model)
        {

            var gt = _waitListRepository.GetGuestType();
            var jsonResult = gt.Select(g => new
            {
                GuestTypeId = g.GuestTypeId,
                GuestType = g.GuestType1

            });
            return new HttpResponseMessage(HttpStatusCode.OK) { Content = new JsonContent(jsonResult) };
        }
    }
}
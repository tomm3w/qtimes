using common.api;
using common.api.Infrastructure;
using common.api.Infrastructure.Exeptions;
using Core.Extensions;
using SeatQ.core.api.Commands.AddVisitMessage;
using SeatQ.core.api.Commands.DeleteVisitMessage;
using SeatQ.core.api.Commands.EditMessages;
using SeatQ.core.api.Commands.EditReadyMessage;
using SeatQ.core.api.Commands.EditVisitMessage;
using SeatQ.core.api.Exceptions;
using SeatQ.core.api.Queries.GetReadyMessages;
using SeatQ.core.api.Queries.GetVisitMessages;
using SeatQ.core.dal.Infrastructure.Repositories;
using SeatQ.core.dal.Models;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using userinfo.dal.Repositories;

namespace SeatQ.core.api.Controllers
{
    [Filters.InvalidPostDataExceptionFilter]
    public class MessageInfoController : SpecificApiController
    {

        private readonly IUserRepository _userRepository;
        private readonly IAccountInfoRepository _accountInfoRepository;

        public MessageInfoController(IApplicationFacade facade, IUserRepository userRepository, IAccountInfoRepository accountInfoRepository)
            : base(facade)
        {
            _userRepository = userRepository;
            _accountInfoRepository = accountInfoRepository;
        }


        [Route("api/messages")]
        [Authorize(Roles = "Administrator,Regional Manager")]
        [EnableCors(origins: "*", headers: "*", methods: "*", SupportsCredentials = true)]
        public HttpResponseMessage Get(int? RestaurantChainId)
        {

            var rMsgRequest = new GetReadyMessagesRequest((int)RestaurantChainId);
            var rMsg = Facade.Query<GetReadyMessagesResponse, GetReadyMessagesRequest>(rMsgRequest);

            var jsonResult = rMsg.Model.Select(m => new
            {
                ReadyMessageId = m.ReadyMessageId,
                RestaurantChainId = m.RestaurantChainId,
                ReadyMessage1 = m.ReadyMessage1,
                IsEnabled = m.IsEnabled,
                IsDeleted = m.IsDeleted,
                ModifiedDate = m.ModifiedDate.ToUniversalTimeString()
            });

            var vMsgRequest = new GetVisitMessagesRequest((int)RestaurantChainId);
            var vMsg = Facade.Query<GetVisitMessagesResponse, GetVisitMessagesRequest>(vMsgRequest);

            var jsonVResult = vMsg.Model.Select(m => new
            {
                VisitMessageId = m.VisitMessageId,
                RestaurantChainId = m.RestaurantChainId,
                Visit = m.Visit,
                VisitMessage1 = m.VisitMessage1,
                IsEnabled = m.IsEnabled,
                IsDeleted = m.IsDeleted
            }).OrderBy(x => x.Visit);

            return CreateHttpResponse(new { ReadyMessage = jsonResult, VisitMessage = jsonVResult });

            /*
            var msg = _messageRepository.GetReadyMessage(model);
            if (msg.Count == 0)
            {
                _messageRepository.InsertReadyMessage(model);
                msg = _messageRepository.GetReadyMessage(model);
            }

            var jsonResult = msg.Select(m => new
            {
                ReadyMessageId = m.ReadyMessageId,
                ReadyMessage = m.ReadyMessage1,
                IsEnabled = m.IsEnabled,
                IsDeleted = m.IsDeleted,
                ModifiedDate = m.ModifiedDate.ToUniversalTimeString()
            });

            var vmsg = _messageRepository.GetVisitMessage(model);

            var jsonVResult = vmsg.Select(m => new
            {
                VisitMessageId = m.VisitMessageId,
                RestaurantChainId = m.RestaurantChainId,
                Visit = m.Visit,
                VisitMessage1 = m.VisitMessage1,
                IsEnabled = m.IsEnabled,
                IsDeleted = m.IsDeleted
            });

            return new HttpResponseMessage(HttpStatusCode.OK) { Content = new JsonContent(new { ReadyMessage = jsonResult, VisitMessage = jsonVResult }) };
        
             */
        }


        [Route("api/messages/ready/")]
        [Authorize(Roles = "Administrator,Regional Manager")]
        [EnableCors(origins: "*", headers: "*", methods: "*", SupportsCredentials = true)]
        [HttpGet]
        public HttpResponseMessage ReadyMessage(int restaurantChainId)
        {
            var rMsgRequest = new GetReadyMessagesRequest(restaurantChainId);
            var rMsg = Facade.Query<GetReadyMessagesResponse, GetReadyMessagesRequest>(rMsgRequest);

            if (rMsg == null)
            {
                //insert default message
                rMsg = Facade.Query<GetReadyMessagesResponse, GetReadyMessagesRequest>(rMsgRequest);
            }

            return new HttpResponseMessage(HttpStatusCode.OK) { Content = new JsonContent(rMsg) };
            /*
            var msg = _messageRepository.GetReadyMessage(model);
            if (msg.Count == 0)
            {
                _messageRepository.InsertReadyMessage(model);
                msg = _messageRepository.GetReadyMessage(model);
            }

            var jsonResult = msg.Select(m => new
            {
                ReadyMessageId = m.ReadyMessageId,
                ReadyMessage = m.ReadyMessage1,
                IsEnabled = m.IsEnabled,
                IsDeleted = m.IsDeleted,
                ModifiedDate = m.ModifiedDate.ToUniversalTimeString()
            });

            return new HttpResponseMessage(HttpStatusCode.OK) { Content = new JsonContent(jsonResult) };
             */
        }

        [Route("api/messages/visit/")]
        [Authorize(Roles = "Administrator,Regional Manager")]
        [EnableCors(origins: "*", headers: "*", methods: "*", SupportsCredentials = true)]
        [HttpGet]
        public HttpResponseMessage VisitMessage(int restaurantChainId)
        {
            var vMsgRequest = new GetVisitMessagesRequest(restaurantChainId);
            var vMsg = Facade.Query<GetVisitMessagesResponse, GetVisitMessagesRequest>(vMsgRequest);

            return new HttpResponseMessage(HttpStatusCode.OK) { Content = new JsonContent(vMsg) };
            /*
            var msg = _messageRepository.GetVisitMessage(model);
            if (msg != null)
            {
                var jsonResult = msg.Select(m => new
                {
                    VisitMessageId = m.VisitMessageId,
                    VisitMessage = m.VisitMessage1,
                    IsEnabled = m.IsEnabled,
                    ModifiedDate = m.ModifiedDate.ToUniversalTimeString(),
                    IsDeleted = m.IsDeleted
                });

                return new HttpResponseMessage(HttpStatusCode.OK) { Content = new JsonContent(jsonResult) };
            }
            return new HttpResponseMessage(HttpStatusCode.NotFound);
             */
        }

        [Route("api/messages")]
        [Authorize(Roles = "Administrator,Regional Manager")]
        [EnableCors(origins: "*", headers: "*", methods: "*", SupportsCredentials = true)]
        public HttpResponseMessage Put(AllMessagesModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var request = new EditMessagesRequest(model);
                    Facade.Command(request);

                    return CreateHttpResponse(new { }, HttpStatusCode.OK);
                }
                catch (ApiException exception)
                {
                    return CreateHttpResponse(exception);
                }
            }
            throw new InvalidPostDataException(ModelState);


            /*
            var msg = _messageRepository.UpdateReadyMessage(model.ReadyMessage, (int)model.RestaurantChainId);
            var vmsg = _messageRepository.UpdateVisitMessag(model.VisitMessage, (int)model.RestaurantChainId);

            var json = new
            {
                ReadyMessag = msg.Select(m => new
                {
                    ReadyMessageId = m.ReadyMessageId,
                    ReadyMessage = m.ReadyMessage1,
                    IsEnabled = m.IsEnabled,
                    IsDeleted = m.IsDeleted,
                    ModifiedDate = m.ModifiedDate.ToUniversalTimeString()
                }),
                VisitMessage = vmsg.Select(m => new
                {
                    VisitMessageId = m.VisitMessageId,
                    VisitMessage = m.VisitMessage1,
                    IsEnabled = m.IsEnabled,
                    IsDeleted = m.IsDeleted
                })
            };

            return new HttpResponseMessage(HttpStatusCode.OK) { Content = new JsonContent(json) };
            */
        }

        [Route("api/messages/visit")]
        [Authorize(Roles = "Administrator,Regional Manager")]
        [EnableCors(origins: "*", headers: "*", methods: "*", SupportsCredentials = true)]
        public HttpResponseMessage Post(VisitMessage model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var request = new AddVisitMessageRequest(model);
                    Facade.Command(request);

                    return CreateHttpResponse(new { }, HttpStatusCode.OK);
                }
                catch (ApiException exception)
                {
                    return CreateHttpResponse(exception);
                }
            }
            throw new InvalidPostDataException(ModelState);

        }

        [Route("api/messages/visit")]
        [Authorize(Roles = "Administrator,Regional Manager")]
        [EnableCors(origins: "*", headers: "*", methods: "*", SupportsCredentials = true)]
        public HttpResponseMessage Put(VisitMessage model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var request = new EditVisitMessageRequest(model);
                    Facade.Command(request);

                    return CreateHttpResponse(new { }, HttpStatusCode.OK);
                }
                catch (ApiException exception)
                {
                    return CreateHttpResponse(exception);
                }
            }
            throw new InvalidPostDataException(ModelState);

        }

        [Route("api/messages/visit")]
        [Authorize(Roles = "Administrator,Regional Manager")]
        [EnableCors(origins: "*", headers: "*", methods: "*", SupportsCredentials = true)]
        public HttpResponseMessage Delete(int VisitMessageId)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var request = new DeleteVisitMessageRequest(VisitMessageId);
                    Facade.Command(request);

                    return CreateHttpResponse(new { }, HttpStatusCode.OK);
                }
                catch (ApiException exception)
                {
                    return CreateHttpResponse(exception);
                }
            }
            throw new InvalidPostDataException(ModelState);

        }

        [Route("api/messages/ready")]
        [Authorize(Roles = "Administrator,Regional Manager")]
        [EnableCors(origins: "*", headers: "*", methods: "*", SupportsCredentials = true)]
        public HttpResponseMessage Put(ReadyMessage model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var request = new EditReadyMessageRequest(model);
                    Facade.Command(request);

                    return CreateHttpResponse(new { }, HttpStatusCode.OK);
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
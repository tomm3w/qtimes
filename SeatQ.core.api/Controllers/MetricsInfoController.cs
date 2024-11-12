using SeatQ.core.api.Infrastructure;
using SeatQ.core.dal.Infrastructure.Repositories;
using SeatQ.core.dal.Models;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Data.Entity.Core.Objects;
using common.api.Infrastructure;
using common.api;
using userinfo.dal.Repositories;

namespace SeatQ.core.api.Controllers
{

    public class MetricsInfoController : SpecificApiController
    {
        private readonly IUserRepository _userRepository;
        private readonly IAccountInfoRepository _accountInfoRepository;

        public MetricsInfoController(IApplicationFacade facade, IUserRepository userRepository, IAccountInfoRepository accountInfoRepository)
            : base(facade)
        {
            _userRepository = userRepository;
            _accountInfoRepository = accountInfoRepository;
        }


        /// <summary>
        /// Get Metrices Info
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("api/metrices/")]
        [Authorize(Roles = "Administrator, Regional Manager, User")]
        [EnableCors(origins: "*", headers: "*", methods: "GET", SupportsCredentials = true)]
        public HttpResponseMessage Get([FromUri]MetricsModel model)
        {
            var SeatedOutputParameter = new ObjectParameter("seated", typeof(int));
            var NoshowOutputParameter = new ObjectParameter("noShow", typeof(int));
            var PromotionsOutputParameter = new ObjectParameter("promotions", typeof(int));
            var GuestOutputParameter = new ObjectParameter("guest", typeof(int));
            var GroupsizeOutputParameter = new ObjectParameter("groupSize", typeof(int));
            var AveragewaittimeOutputParameter = new ObjectParameter("averageWaitTime", typeof(int));


            using (SeatQEntities context = new SeatQEntities())
            {
                context.GetMetrics(model.RestaurantChainId, model.MetricsType, model.StartDate, model.EndDate, SeatedOutputParameter, NoshowOutputParameter, PromotionsOutputParameter, GuestOutputParameter, GroupsizeOutputParameter, AveragewaittimeOutputParameter);
            }

            var jsonResult = new
            {
                Seated = SeatedOutputParameter.Value,
                NoShow = NoshowOutputParameter.Value,
                Promotions = PromotionsOutputParameter.Value,
                Guest = GuestOutputParameter.Value,
                GroupSize = GroupsizeOutputParameter.Value,
                AverageWaitTime = AveragewaittimeOutputParameter.Value
            };

            return CreateHttpResponse(jsonResult);

        }

        [Route("api/ispasswordchanged/")]
        [Authorize(Roles = "Administrator, Regional Manager, User")]
        [EnableCors(origins: "*", headers: "*", methods: "GET", SupportsCredentials = true)]
        public HttpResponseMessage Get()
        {
            bool usr = _accountInfoRepository.IsPasswordChanged(Authentication.Web.Security.CurrentUserId);
            var jsonResult = new
            {
                IsPasswordChanged = usr
            };

            return CreateHttpResponse(jsonResult);
        }
    }
}
using iVeew.common.api;
using iVeew.common.api.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using QTimes.api.Infrastructure;
using QTimes.api.Queries.Reservation.GetBusinessAccounts;
using QTimes.api.Queries.Reservation.GetBusinessProfile;
using QTimes.api.Queries.Reservation.GetConcertById;
using System;

namespace QTimes.api.Controllers
{
    //TODO: require refactoring
    [EnableCors("AllowSpecificOrigin")]
    [Authorize]
    [ValidateModel]
    [ApiController]
    [Route("[controller]")]
    public class ConcertReservationSettingController : SpecificApiController
    {
        public ConcertReservationSettingController(IApplicationFacade applicationFacade)
          : base(applicationFacade)
        {

        }

        [Route("")]
        [HttpGet]
        public IActionResult Get()
        {
            var response = Facade.Query<GetBusinessProfileResponse, GetBusinessProfileRequest>(new GetBusinessProfileRequest { UserId = UserId, UserName = Username });
            return CreateHttpResponse(response);
        }

        [Route("/concert/{id}")]
        [HttpGet]
        public IActionResult GetConcert(string id)
        {
            var response = Facade.Query<GetConcertByIdResponse, GetConcertByIdRequest>(new GetConcertByIdRequest { UserId = UserId, Id = Guid.Parse(id), UserName = Username });
            return CreateHttpResponse(response);
        }

        [Route("businessaccounts")]
        [HttpGet]
        public IActionResult GetBusinessAccounts()
        {
            var response = Facade.Query<GetBusinessAccountsResponse, GetBusinessAccountsRequest>(new GetBusinessAccountsRequest { UserId = UserId });
            return CreateHttpResponse(response);
        }

    }

    }

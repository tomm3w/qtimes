using iVeew.common.api;
using iVeew.common.api.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QTimes.api.Commands.Reservation.UpdateConcertAccountSetup;
using QTimes.api.Commands.Reservation.UpdateConcertBusinessProfile;
using QTimes.api.Infrastructure;
using System;

namespace QTimes.api.Controllers
{
    //[Authorize]
    //[ValidateModel]
    [ApiController]
    [Route("[controller]")]
    public class ConcertController : SpecificApiController
    {
        public ConcertController(IApplicationFacade applicationFacade)
          : base(applicationFacade)
        {

        }

        [Route("/concerts/{concertId:Guid}/profile")]
        [HttpPut]
        public IActionResult UpdateProfile([FromForm] UpdateConcertBusinessProfileRequest request, Guid concertId)
        {
            request.UserId = UserId;
            request.ConcertId = concertId;
            Facade.Command(request);

            return CreateOkHttpResponse();
        }
        [Route("/concerts/{concertId:Guid}/account")]
        [HttpPut]
        public IActionResult UpdateConcertAccount([FromForm] UpdateConcertAccountSetupRequest model)
        {
            model.UserId = UserId;
            Facade.Command(model);
            return CreateOkHttpResponse();
        }
    }

    }

using common.api;
using common.api.Infrastructure;
using Core.Helpers;
using SeatQ.core.api.Commands.Reservation.AddConcertSeating;
using SeatQ.core.api.Commands.Reservation.UpdateConcertAccountSetup;
using SeatQ.core.api.Commands.Reservation.UpdateConcertBusinessProfile;
using SeatQ.core.api.Commands.Reservation.UpdateConcertPreference;
using SeatQ.core.api.Infrastructure;
using SeatQ.core.api.Queries.Reservation.GetBusinessAccounts;
using SeatQ.core.api.Queries.Reservation.GetBusinessProfile;
using SeatQ.core.api.Queries.Reservation.GetConcertById;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace SeatQ.core.api.Controllers
{
    [RoutePrefix("api/concertreservationsetting")]
    [Authorize(Roles = "Administrator,Regional Manager")]
    [ValidateModel]
    public class ConcertReservationSettingController : SpecificApiController
    {
        public ConcertReservationSettingController(IApplicationFacade applicationFacade)
          : base(applicationFacade)
        { }

        [Route("")]
        public HttpResponseMessage Get()
        {
            var response = Facade.Query<GetBusinessProfileResponse, GetBusinessProfileRequest>(new GetBusinessProfileRequest { UserId = UserId, UserName = Username });
            return CreateHttpResponse(response);
        }

        [Route("concert/{id}")]
        public HttpResponseMessage GetConcert(string id)
        {

            var response = Facade.Query<GetConcertByIdResponse, GetConcertByIdRequest>(new GetConcertByIdRequest { UserId = UserId, Id = Guid.Parse(id), UserName = Username });
            return CreateHttpResponse(response);
        }

        [Route("businessaccounts")]
        public HttpResponseMessage GetBusinessAccounts()
        {
            var response = Facade.Query<GetBusinessAccountsResponse, GetBusinessAccountsRequest>(new GetBusinessAccountsRequest { UserId = UserId });
            return CreateHttpResponse(response);
        }

        [Route("UpdateConcertAccount")]
        [HttpPut]
        public HttpResponseMessage UpdateConcertAccount(UpdateConcertAccountSetupRequest model)
        {
            model.UserId = UserId;
            Facade.Command(model);
            return CreateHttpResponse(HttpStatusCode.OK);
        }

        [Route("UpdateConcertPreference")]
        [HttpPut]
        public HttpResponseMessage UpdateConcertPreference(UpdateConcertPreferenceRequest model)
        {
            model.UserId = UserId;
            Facade.Command(model);
            return CreateHttpResponse(HttpStatusCode.OK);
        }

        [Route("UpdateConcertProfile")]
        [HttpPut]
        public HttpResponseMessage UpdateProfile(UpdateConcertBusinessProfileRequest model)
        {
            model.UserId = UserId;
            Facade.Command(model);

            try
            {
                var TempPATH = HttpContext.Current.Server.MapPath("~/content/uploads/reservation/temp/");
                var PATH = HttpContext.Current.Server.MapPath("~/content/uploads/reservation/");

                if (model.ImagePath != null)
                    ImageHelper.MoveFile(Path.Combine(TempPATH, model.ImagePath), Path.Combine(PATH, model.ImagePath));
                if (model.PrivacyPolicyFilePath != null)
                    ImageHelper.MoveFile(Path.Combine(TempPATH, model.PrivacyPolicyFilePath), Path.Combine(PATH, model.PrivacyPolicyFilePath));
                if (model.ServiceTermsFilePath != null)
                    ImageHelper.MoveFile(Path.Combine(TempPATH, model.ServiceTermsFilePath), Path.Combine(PATH, model.ServiceTermsFilePath));
                if (model.CummunityGuidelinesFilePath != null)
                    ImageHelper.MoveFile(Path.Combine(TempPATH, model.CummunityGuidelinesFilePath), Path.Combine(PATH, model.CummunityGuidelinesFilePath));
            }
            catch (Exception ex)
            {
            }

            return CreateHttpResponse(HttpStatusCode.OK);
        }

        [Route("UpdateConcertSeating")]
        [HttpPut]
        public HttpResponseMessage UpdateConcertSeating(AddConcertSeatingRequest model)
        {
            model.UserId = UserId;
            Facade.Command(model);

            try
            {
                var TempPATH = HttpContext.Current.Server.MapPath("~/content/uploads/reservation/temp/");
                var PATH = HttpContext.Current.Server.MapPath("~/content/uploads/reservation/");

                if (model.SeatMapPath != null)
                    ImageHelper.MoveFile(Path.Combine(TempPATH, model.SeatMapPath), Path.Combine(PATH, model.SeatMapPath));

            }
            catch (Exception ex)
            {
            }

            return CreateHttpResponse(HttpStatusCode.OK);
        }
    }
}

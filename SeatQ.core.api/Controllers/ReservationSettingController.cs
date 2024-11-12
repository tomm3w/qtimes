using common.api;
using common.api.Infrastructure;
using Core.Helpers;
using SeatQ.core.api.Commands.Reservation.AccountSetup;
using SeatQ.core.api.Commands.Reservation.BusinessProfile;
using SeatQ.core.api.Commands.Reservation.Preference;
using SeatQ.core.api.Infrastructure;
using SeatQ.core.api.Queries.Reservation.GetAccount;
using SeatQ.core.api.Queries.Reservation.GetBusinessProfile;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace SeatQ.core.api.Controllers
{
    [RoutePrefix("api/reservationsetting")]
    [Authorize(Roles = "Administrator,Regional Manager")]
    [ValidateModel]
    public class ReservationSettingController : SpecificApiController
    {
        public ReservationSettingController(IApplicationFacade applicationFacade)
           : base(applicationFacade)
        { }

        [Route("")]
        public HttpResponseMessage Get()
        {
            var response = Facade.Query<GetAccountResponse, GetAccountRequest>(new GetAccountRequest { UserId = UserId });
            return CreateHttpResponse(response);
        }

        [Route("GetBusiness/{Id}")]
        public HttpResponseMessage GetBusiness(string Id)
        {
            var response = Facade.Query<GetBusinessProfileResponse, GetBusinessProfileRequest>(new GetBusinessProfileRequest { UserId = UserId, UserName = Username, Id = Guid.Parse(Id) });
            return CreateHttpResponse(response);
        }

        [Route("")]
        public HttpResponseMessage Put(AccountSetupRequest model)
        {
            model.UserId = UserId;
            Facade.Command(model);
            return CreateHttpResponse(HttpStatusCode.OK);
        }

        [Route("UpdatePreference")]
        [HttpPut]
        public HttpResponseMessage UpdatePreference(PreferenceRequest model)
        {
            model.UserId = UserId;
            Facade.Command(model);
            return CreateHttpResponse(HttpStatusCode.OK);
        }

        [Route("UpdateProfile")]
        [HttpPut]
        public HttpResponseMessage UpdateProfile(BusinessProfileRequest model)
        {
            model.UserId = UserId;
            Facade.Command(model);

            try
            {
                var TempPATH = HttpContext.Current.Server.MapPath("~/content/uploads/reservation/temp/");
                var PATH = HttpContext.Current.Server.MapPath("~/content/uploads/reservation/");
                File.Move(Path.Combine(TempPATH, model.LogoPath), Path.Combine(PATH, model.LogoPath));
            }
            catch (Exception ex)
            {
            }

            return CreateHttpResponse(HttpStatusCode.OK);
        }

        [Route("upload")]
        [HttpPost]
        public async Task<HttpResponseMessage> UploadAccount()
        {
            if (Request.Content.IsMimeMultipartContent())
            {

                string fileName = string.Empty;
                var PATH = HttpContext.Current.Server.MapPath("~/content/uploads/reservation/temp/");
                var streamProvider = new MultipartFormDataStreamProvider(PATH);
                await Request.Content.ReadAsMultipartAsync(streamProvider);
                foreach (MultipartFileData fileData in streamProvider.FileData)
                {
                    if (string.IsNullOrEmpty(fileData.Headers.ContentDisposition.FileName))
                    {
                        return CreateHttpResponse("This request is not properly formatted", HttpStatusCode.BadRequest);
                    }
                    fileName = fileData.Headers.ContentDisposition.FileName;
                    if (fileName.StartsWith("\"") && fileName.EndsWith("\""))
                    {
                        fileName = fileName.Trim('"');
                    }
                    if (fileName.Contains(@"/") || fileName.Contains(@"\"))
                    {
                        fileName = Path.GetFileName(fileName);
                    }

                    fileName = Guid.NewGuid().ToString() + Path.GetExtension(fileName);
                    try
                    {
                        ImageHelper.TryDeleteFile(PATH + fileName);
                        File.Move(fileData.LocalFileName, Path.Combine(PATH, fileName));
                    }
                    catch (Exception ex)
                    {
                        return CreateHttpResponse(ex.Message);
                    }

                }

                Uri myuri = new Uri(HttpContext.Current.Request.Url.AbsoluteUri);
                string pathQuery = myuri.PathAndQuery;
                string hostName = myuri.ToString().Replace(pathQuery, "");
                return CreateHttpResponse(new { ImageName = fileName, ImageFullPath = hostName + @"/content/uploads/reservation/temp/" + fileName }, HttpStatusCode.OK);
            }
            else
                return CreateHttpResponse("Invalid", HttpStatusCode.BadRequest);
        }
    }
}

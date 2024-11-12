using iVeew.common.api;
using iVeew.common.api.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QTimes.api.Commands.Reservation.AccountSetup;
using QTimes.api.Commands.Reservation.BusinessProfile;
using QTimes.api.Commands.Reservation.Preference;
using QTimes.api.Infrastructure;
using QTimes.api.Queries.Reservation.GetAccount;
using QTimes.api.Queries.Reservation.GetBusinessProfile;
using System;
using System.IO;
using System.Threading.Tasks;

namespace QTimes.api.Controllers
{
    //[RoutePrefix("api/reservationsetting")]
    //[EnableCors("MyPolicy")]
    [Authorize]
    [ValidateModel]
    [ApiController]
    [Route("[controller]")]
    public class ReservationSettingController : SpecificApiController
    {
        private IWebHostEnvironment _webHostEnvironment;
        private IBlobService _blobService;
        public ReservationSettingController(IApplicationFacade applicationFacade, IWebHostEnvironment webHostEnvironment, IBlobService blobService)
           : base(applicationFacade)
        {
            _webHostEnvironment = webHostEnvironment;
            _blobService = blobService;
        }

        [Route("")]
        [HttpGet]
        public IActionResult Get()
        {
            var response = Facade.Query<GetAccountResponse, GetAccountRequest>(new GetAccountRequest { UserId = UserId, UserName = Username });
            return CreateHttpResponse(response);
        }

        [Route("GetBusiness/{Id}")]
        [HttpGet]
        public IActionResult GetBusiness(string Id)
        {
            var response = Facade.Query<GetBusinessProfileResponse, GetBusinessProfileRequest>(new GetBusinessProfileRequest { UserId = UserId, UserName = Username, Id = Guid.Parse(Id) });
            return CreateHttpResponse(response);
        }

        [Route("")]
        [HttpPut]
        public IActionResult Put([FromForm] AccountSetupRequest model)
        {
            model.UserId = UserId;
            Facade.Command(model);
            return CreateOkHttpResponse();
        }

        [Route("UpdatePreference")]
        [HttpPut]
        public IActionResult UpdatePreference([FromForm] PreferenceRequest model)
        {
            model.UserId = UserId;
            Facade.Command(model);
            return CreateOkHttpResponse();
        }

        [Route("UpdateProfile")]
        [HttpPut]
        public IActionResult UpdateProfile([FromForm] BusinessProfileRequest model)
        {
            model.UserId = UserId;
            Facade.Command(model);

            return CreateOkHttpResponse();
        }

        [HttpPost("upload", Name = "upload")]
        public async Task<IActionResult> UploadAccount([FromForm] IFormFile file0)
        {
            try
            {
                var file = file0;
                string fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";


                using (MemoryStream memoryStream = new MemoryStream())
                {
                    await file.CopyToAsync(memoryStream);
                    memoryStream.Position = 0;
                    var url = await _blobService.UploadFile(memoryStream, fileName);
                    return CreateHttpResponse(new { ImageName = fileName, ImageFullPath = $"{url}" });
                }
            }
            catch (Exception ex)
            {
                return CreateBadRequestHttpResponse(ex.Message);
            }
        }
    }
}

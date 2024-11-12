using iVeew.common.api;
using iVeew.common.api.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using QTimes.api.Infrastructure;
using QTimes.api.Queries;
using System;
using System.Threading.Tasks;

namespace SeatQ.core.api.Controllers
{
    [ValidateModel]
    [AllowAnonymous]
    [ApiController]
    [Route("[controller]")]
    public class DriverLicenseController : SpecificApiController
    {
        private readonly IDistributedCache _cache;
        public DriverLicenseController(IApplicationFacade applicationFacade, IDistributedCache cache)
           : base(applicationFacade)
        {
            _cache = cache;
        }

        [Route("")]
        [HttpPost]
        public IActionResult Post([FromBody] ParseDLRequest request)
        {
            var response = Facade.Query<ParseDLResponse, ParseDLRequest>(request);
            return CreateHttpResponse(response);
        }
        [Route("")]
        [HttpGet]
        public async Task<IActionResult> Get(Guid id)
        {
            var str = await _cache.GetStringAsync(id.ToString());
            var response = JsonConvert.DeserializeObject<ParseDLResponse>(str);
            //var response = await _redisCacheClient.Db0.GetAsync<ParseDLResponse>(id.ToString());
            return CreateHttpResponse(response);
        }
    }
}

using iVeew.common.api.Queries;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace QTimes.api.Queries
{
    public class ParseDLQuery : IQuery<ParseDLResponse, ParseDLRequest>
    {
        private readonly IRestClient _restClient;
        private readonly IConfiguration _configuration;
        private readonly IDistributedCache _cache;
        public ParseDLQuery(IRestClient restClient, IConfiguration configuration, IDistributedCache cache)
        {
            _restClient = restClient;
            _cache = cache;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            _configuration = configuration;
        }

        public ParseDLResponse Handle(ParseDLRequest request)
        {
            var parsed = Task.Run(async () => await GetFrontDL(request)).Result;
            var dvs = new ParseDLResponse();
            dvs.FacePhotoFromDocument = parsed.Document.Picture.ImageBase64;
            dvs.Document = new DVSDocument();
            dvs.Document.City = parsed.Document.City.Value;
            dvs.Document.State = parsed.Document.State.Value;
            dvs.Document.Zip = parsed.Document.Zip.Value;
            dvs.Document.Address = parsed.Document.Address.Value;
            dvs.Document.FirstName = parsed.Document.FirstName.Value;
            dvs.Document.FamilyName = parsed.Document.FamilyName.Value;
            dvs.Document.MiddleName = parsed.Document.MiddleName.Value;
            dvs.Document.FullName = parsed.Document.FullName.Value;
            dvs.Document.DOB = parsed.Document.DOB.Value;
            var key = Guid.NewGuid().ToString();
            dvs.RefId = key;

            Task.Run(async () => await _cache.SetStringAsync(dvs.RefId, JsonConvert.SerializeObject(dvs), new DistributedCacheEntryOptions()
            {
                AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(30)
            })).Wait();
            return dvs;
        }
        private async Task<ParseFrontDL> GetFrontDL(ParseDLRequest request)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(_configuration["Dvs:apiUrl"]);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("ApiKey", _configuration["Dvs:secretKey"]);

            var content = new MultipartFormDataContent();
            content.Add(request.FrontByteArrayContent, "image", "image.jpg");
            content.Add(new StringContent("true"), "ReturnSignatureImage");
            content.Add(new StringContent("true"), "ReturnPhotoImage");

            HttpResponseMessage response = await client.PostAsync("api/ocr/recognize", content);
            var result = response.Content != null ? await response.Content.ReadAsStringAsync() : null;

            if (response.IsSuccessStatusCode)
            {
                var o = JsonConvert.DeserializeObject<List<ParseFrontDL>>(result);
                return o.First();
            }
            else
                return new ParseFrontDL
                {
                    ErrorMsg = result
                };

        }
    }
}
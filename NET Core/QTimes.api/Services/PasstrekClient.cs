using iVeew.common.api.Infrastructure;
using Newtonsoft.Json.Linq;
using QTimes.api.Models.Pass;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using JsonContent = iVeew.common.api.Infrastructure.JsonContent;

namespace QTimes.api.Services
{
    public sealed class PasstrekClient : IPasstrekClient
    {
        private readonly Microsoft.Extensions.Configuration.IConfiguration _configuration;
        public PasstrekClient(Microsoft.Extensions.Configuration.IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<string> GeneratePassUrlAsync(QTimesPassModel passModel)
        {
            string passtrekUrl = _configuration["PasstrekUrl"];

            try
            {
                if (!string.IsNullOrWhiteSpace(passtrekUrl))
                {
                    using (HttpClient client = new HttpClient())
                    {
                        var response = await client.PostAsync(passtrekUrl, new JsonContent(passModel));

                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            var contentAsString = await response.Content.ReadAsStringAsync();
                            JObject obj = JObject.Parse(contentAsString);
                            return obj.Value<string>("data").ToString();
                        }
                    }
                }

                return null;
            }
            catch
            {
                return null;
            }
        }
    }
}
using common.api.Infrastructure;
using Newtonsoft.Json.Linq;
using SeatQ.core.api.Models.Pass;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace SeatQ.core.api.Services
{
    public sealed class PasstrekClient : IPasstrekClient
    {
        public async Task<string> GeneratePassUrlAsync(QTimesPassModel passModel)
        {
            string passtrekUrl = ConfigurationManager.AppSettings["PasstrekUrl"];

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
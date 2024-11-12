using System;
using core.api.client.Models;
using Newtonsoft.Json;
using RestSharp;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;

namespace core.api.client
{
    public class CoreClient : ICoreClient
    {
        private readonly RestClient _client;

        public CoreClient()
        {
            _client = new RestClient(ConfigurationManager.AppSettings["coreApiEndpoint"]);
        }

        public bool Register(RegisterModel model)
        {
            var request = new RestRequest("account/register", Method.POST) { RequestFormat = DataFormat.Json };
            request.AddBody(model);

            var response = _client.Post(request);
            var result = JsonConvert.DeserializeObject<ApiExceptionResponse>(response.Content);
            if (result.Type != ApiResponseType.data)
            {
                throw new CoreClientException(result.Message);
            }
            return true;
        }

        public bool Confirm(string confirmationCode)
        {
            var request = new RestRequest("account/confirm", Method.GET);
            request.AddParameter(new Parameter() { Name = "confirmationCode", Value = confirmationCode, Type = ParameterType.QueryString });
            var response = _client.Get(request);
            return response.StatusCode == HttpStatusCode.OK;
        }

        public void ResetPassword(string email, string registrationSource)
        {
            var request = new RestRequest("account/reset", Method.GET);
            request.AddParameter(new Parameter() { Name = "Email", Value = email, Type = ParameterType.GetOrPost });
            request.AddParameter(new Parameter() { Name = "registrationSource", Value = registrationSource, Type = ParameterType.GetOrPost });
            var response = _client.Get(request);
            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                var result = JsonConvert.DeserializeObject<ApiExceptionResponse>(response.Content);
                throw new CoreClientException(result.Message);
            }
            if (response.StatusCode == HttpStatusCode.InternalServerError)
            {
                throw new CoreClientException("Somthing went wrong");
            }
        }

        public User GetUser(string token)
        {
            var request = new RestRequest("user", Method.GET);
            request.AddHeader("Authorization", token);
            var response = _client.Get(request);
            var result = JsonConvert.DeserializeObject<UserResponse>(response.Content);

            if (result.Data == null)
                throw new InvalidOperationException("Core api is not available!");

            return result.Data.User;
        }

        public void SendEmail(List<EmailLinkedResourceItem> linkedResourceItems, Dictionary<string, string> replaceTokens, string templatePath,
            List<string> emailTo, string emailSubject, string registrationSorce, List<EmailLinkedResourceItem> linkedResourcesPaths)
        {

            var request = new RestRequest("email", Method.POST);
            request.AddFile("Template", templatePath);
            request.AddParameter("EmailTo", JsonConvert.SerializeObject(emailTo), ParameterType.GetOrPost);
            request.AddParameter("RegistrationSource", "passtrek", ParameterType.GetOrPost);
            request.AddParameter("EmailSubject", "Share pass", ParameterType.GetOrPost);

            request.AddParameter("EmailLinkedResourceItems", JsonConvert.SerializeObject(linkedResourcesPaths),
                ParameterType.GetOrPost);
            request.AddParameter("TokenItems", JsonConvert.SerializeObject(replaceTokens),
                ParameterType.GetOrPost);
            for (int i = 0; i < linkedResourcesPaths.Count; i++)
            {
                var emailLinkedResourceItem = linkedResourcesPaths[i];
                request.AddFile(string.Format("LinkedResources[{0}]", i), emailLinkedResourceItem.Path);
            }
            var response = _client.Post(request);
            if (response.StatusCode != HttpStatusCode.OK)
                throw new CoreClientException(response.Content);
        }
        public List<int> GetBusinesses(string token)
        {
            RestRequest restRequest = new RestRequest("business/names", Method.GET);
            restRequest.AddHeader("Authorization", token);
            restRequest.AddQueryParameter("page", "1");
            restRequest.AddQueryParameter("pageSize", int.MaxValue.ToString());
            var result = _client.Get(restRequest);
            if (!string.IsNullOrWhiteSpace(result.Content))
                return JsonConvert.DeserializeObject<List<Business>>(result.Content).Select(x => x.BusinessId).ToList();
            return new List<int>();
        }

        public List<Business> GetBusinesses(string token, int? regionId)
        {
            RestRequest restRequest = new RestRequest("business/names", Method.GET);
            restRequest.AddHeader("Authorization", token);
            restRequest.AddQueryParameter("page", "1");
            restRequest.AddQueryParameter("pageSize", int.MaxValue.ToString());
            if (regionId.HasValue)
                restRequest.AddQueryParameter(nameof(regionId), regionId.Value.ToString());
            var result = _client.Get(restRequest);
            if (!string.IsNullOrWhiteSpace(result.Content))
                return JsonConvert.DeserializeObject<List<Business>>(result.Content).ToList();
            return new List<Business>();
        }
    }

    public class EmailLinkedResourceItem
    {
        public EmailLinkedResourceItem(string path, string contentId)
        {
            Path = path;
            ContentId = contentId;
        }

        public string Path { get; private set; }
        public string ContentId { get; private set; }
    }
}

using System.Collections.Generic;
using core.api.client.Models;

namespace core.api.client
{
    public interface ICoreClient
    {
        bool Register(RegisterModel model);

        bool Confirm(string userKey);

        User GetUser(string token);

        void ResetPassword(string email, string registrationSource);

        List<int> GetBusinesses(string token);

        List<Business> GetBusinesses(string token, int? regionId);

        void SendEmail(List<EmailLinkedResourceItem> linkedResourceItems, Dictionary<string, string> replaceTokens,
            string templatePath, List<string> emailTo, string emailSubject, string registrationSorce,
            List<EmailLinkedResourceItem> linkedResourcesPaths);
    }
}

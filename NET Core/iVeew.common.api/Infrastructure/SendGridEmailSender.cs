using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace iVeew.common.api.Infrastructure
{
    public class SendGridEmailSender : IEmailSender
    {
        private readonly string EMAIL;
        private readonly string NAME;
        private readonly string SENDGRID_API_KEY;
        public SendGridEmailSender(IConfiguration configuration)
        {
            EMAIL = configuration["sendgrid:email"];
            NAME = configuration["sendgrid:name"];
            SENDGRID_API_KEY = configuration["sendgrid:apiKey"];
        }

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            SendGridClient client = new SendGridClient(SENDGRID_API_KEY);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress(EMAIL, NAME),
                Subject = subject,
                //PlainTextContent = email,
                HtmlContent = htmlMessage
            };
            msg.AddTo(new EmailAddress(email));

            // Disable click tracking.
            // See https://sendgrid.com/docs/User_Guide/Settings/tracking.html
            msg.SetClickTracking(false, false);

            return client.SendEmailAsync(msg);
        }
    }
}

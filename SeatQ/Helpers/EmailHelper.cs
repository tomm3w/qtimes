using Core.Extensions;
using Core.Helpers;
using SeatQ.Areas.Admin.Models;
using SeatQ.core.dal.Models;
using System;
using System.Linq;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Web.Configuration;

namespace SeatQ.Helpers
{
    public class EmailHelper
    {
        public static bool IsValid(string email)
        {
            Regex regex = new Regex(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", RegexOptions.Compiled);


            if (string.IsNullOrEmpty(email))
                return false;

            Match match = regex.Match(email);
            return ((match.Success && (match.Index == 0)) && (match.Length == email.Length));
        }
        public void SendMail(MailMessage message)
        {
            SmtpClient client = new SmtpClient();
            message.BodyEncoding = System.Text.Encoding.UTF8;
            message.SubjectEncoding = System.Text.Encoding.UTF8;
            client.SendCompleted += new SendCompletedEventHandler(client_SendCompleted);
            try
            {
                client.Send(message);
            }
            catch (Exception ex)
            {
                //Logger.Log("email sending failed:" + ex.Message + "\n" + ex.StackTrace);

            }

            //Logger.Log("email sending");
        }

        void client_SendCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            MailMessage message = (MailMessage)e.UserState;
            string subject = message.Subject;
            string log = null;
            if (e.Cancelled)
            {
                log = string.Format("Cancelled to send mail \nTo : {0} \nSubject:{1}", message.To.ToList().Join<MailAddress>(m => m.User, ","));
            }
            if (e.Error != null)
            {
                log = string.Format("Error {1} occurred when sending mail [{0}] ", subject, e.Error.ToString());
            }
            else
            {
                log = string.Format("Message [{0}] sent \nTo:{0},\nSubject:{1}.", message.To.ToList().Join(m => m.User), subject);
            }
            //Debug.WriteLine(log);
            //Logger.Log(log);
        }

        public void SendConfirmationEmailMail(UserProfile user)
        {
            try
            {
                MailMessage message = new MailMessage { Subject = "SeatQ Signup - Confirmation Mail" };
                message.To.Add(user.Email);
                message.IsBodyHtml = true;
                String confirmationURL = ConfigurationHelper.Instance.ConfirmationURL;
                message.Body = String.Format("Welcome {0},<br/><br/>Thanks for registering, please click on the link below to activate your account.<br/><br/><a href=\"{1}\">{1}</a><br/><br/>Best Regards,<br/>SeatQ Team", user.FullName, confirmationURL + "/" + user.ConfirmationToken);
                SendMail(message);
            }
            catch (Exception ex)
            {
                throw new Exception("failed");
            }
        }

        public void ContactSupport(ContactModel model)
        {
            try
            {
                MailMessage message = new MailMessage { Subject = "SeatQ Contact" };
                String ContactEmail = WebConfigurationManager.AppSettings["ContactEmail"];//ConfigurationHelper.Instance.ContactEmail;
                message.To.Add(ContactEmail);
                message.IsBodyHtml = true;
                message.Body = String.Format("Please follow for business setup.<br><br>Name: {0}<br/>Business Name: {1}<br/>Email: {2}<br/>Phone: {3}<br/>Message: {4}", model.Name, model.BusinessName, model.Email, model.Phone, model.Message);
                SendMail(message);
            }
            catch (Exception ex)
            {

            }
        }
        public void SendResetPassword(UserProfile user, string pwd)
        {
            MailMessage message = new MailMessage { Subject = "SeatQ Password - Reset Password Mail" };
            message.To.Add(user.Email);
            message.IsBodyHtml = true;
            String confirmationURL = ConfigurationHelper.Instance.ConfirmationURL;
            message.Body = String.Format("Hello {0},<br/><br/>Your new password is <b>{1}</b><br/><br/>Best Regards,<br/>SeatQ Team", user.UserName, pwd);
            SendMail(message);
        }
    }

}
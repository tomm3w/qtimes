using System;
using System.Configuration;
using System.Web;
using System.Web.Security;
using WebMatrix.WebData;
using System.Linq;
using IdentityModel.Client;

namespace Authentication.Web
{
    public class Security
    {
        public static bool Login(string userName, string password, bool persistCookie = false)
        {
            var token = RequestToken(userName, password).AccessToken;
            if (!string.IsNullOrWhiteSpace(token))
            {
                FormsAuthentication.SetAuthCookie(userName, persistCookie);
                var cookie = new HttpCookie("token", token);
                //cookie.Expires = DateTime.Now.AddHours(8);
                HttpContext.Current.Response.SetCookie(cookie);
                return true;
            }
            return false;

        }

        public static void Logout()
        {
            HttpContext.Current.Response.Cookies.Clear();
            FormsAuthentication.SignOut();

        }

        public static MembershipUser CreateUserAndAccount(string userName, string password)
        {
            // we need to save user info on different web app to, we will create TomCoreWebApp for that, 
            //and we will use webservice to saveuserinfo there with appname
            var user = Membership.CreateUser(userName, password);
            Roles.AddUserToRoles(user.UserName, new[] { "User", "IdentityServerUsers" });
            return user;
        }

        public static MembershipUser CreateUserAndRole(string userName, string password, string email)
        {
            // we need to save user info on different web app to, we will create TomCoreWebApp for that, 
            //and we will use webservice to saveuserinfo there with appname
            var user = Membership.CreateUser(userName, password, email);
            Roles.AddUserToRoles(user.UserName, new[] { "Regional Manager", "IdentityServerUsers" });
            return user;
        }
        public static bool ResetPassword(string passwordResetToken, string newPassword)
        {
            return WebSecurity.ResetPassword(passwordResetToken, newPassword);

        }

        public static string GeneratePasswordExpirationToke(string username)
        {
            return WebSecurity.GeneratePasswordResetToken(username);
        }


        public static int GetUserId(string username)
        {
            return WebSecurity.GetUserId(username);
        }

        public static string CreateUserAndAccount(string username, string password, object properties)
        {
            string retVal = WebSecurity.CreateUserAndAccount(username, password, properties);
            return retVal;
        }

        public static bool ChangePassword(string name, string oldPassword, string newPassword)
        {
            return WebSecurity.ChangePassword(name, oldPassword, newPassword);
        }

        public static void CreateAccount(string name, string newPassword)
        {
            WebSecurity.CreateAccount(name, newPassword);
        }

        public static Guid CurrentUserId
        {
            get { return (Guid)Membership.GetUser().ProviderUserKey; }
        }

        public static Guid CurrentUserIdByName(string username)
        {
            return (Guid)Membership.GetUser(username).ProviderUserKey;
        }
        public static string CurrentUserName => WebSecurity.CurrentUserName;

        public static string GeneratePasswordResetToken(string userName)
        {
            return WebSecurity.GeneratePasswordResetToken(userName);
        }

        public static void AddUsersToRoles(string userName, string roleName)
        {
            if (!Roles.GetRolesForUser(userName).Contains(roleName))
            {
                Roles.AddUsersToRoles(new[] { userName }, new[] { roleName });
            }
        }

        public static bool ValidateUser(string userName, string currentPassword)
        {
            return Membership.ValidateUser(userName, currentPassword);
        }

        private static TokenResponse RequestToken(string username, string password)
        {
            TokenClient client = new TokenClient(ConfigurationManager.AppSettings["OAuthTokenEndpoint"], ConfigurationManager.AppSettings["iDestnOAuthClient"],
                ConfigurationManager.AppSettings["iDestnOAuthSecret"]);
            
            var response = client.RequestResourceOwnerPasswordAsync(username, password, "api").Result;
            return response;
        }

    }
}

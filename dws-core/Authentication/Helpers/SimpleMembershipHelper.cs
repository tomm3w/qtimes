using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading;
using System.Web.Security;
using Authentication.Models;
using WebMatrix.WebData;

namespace Authentication.Helpers
{
    
    public static class SimpleMembershipHelper
    {
        private static SimpleMembershipInitializer _initializer;
        private static object _initializerLock = new object();
        private static bool _isInitialized;

        public static void Inititalize()
        {
            LazyInitializer.EnsureInitialized(ref _initializer, ref _isInitialized, ref _initializerLock);
        }
        private class SimpleMembershipInitializer
        {
            public SimpleMembershipInitializer()
            {
                Database.SetInitializer<UsersContext>(null);

                try
                {
                    using (var context = new UsersContext())
                    {
                        if (!context.Database.Exists())
                        {
                            // Create the SimpleMembership database without Entity Framework migration schema
                            ((IObjectContextAdapter)context).ObjectContext.CreateDatabase();
                        }
                        
                    }

                    WebSecurity.InitializeDatabaseConnection("AuthenticationConnection", "User", "UserId", "UserName", autoCreateTables: true);
                    if (!Roles.RoleExists("Administrator"))
                    {
                        Roles.CreateRole("Administrator");
                    }
                    if (!Roles.RoleExists("Regional Manager"))
                    {
                        Roles.CreateRole("Regional Manager");
                    }
                    if (!Roles.RoleExists("User"))
                    {
                        Roles.CreateRole("User");
                    }
                    string username, password, otherUsername, otherPassword;
                    if (ConfigurationHelper.Instance.Environment == ConfigurationHelper.K_ENVIRONMENT_PRODUCTION)
                    {
                        username = ConfigurationHelper.Instance.ProdAdminUserName;
                        password = ConfigurationHelper.Instance.ProdAdminPassword;
                        otherUsername = ConfigurationHelper.Instance.DevAdminUserName;
                        otherPassword = ConfigurationHelper.Instance.DevAdminPassword;
                    }
                    else
                    {
                        username = ConfigurationHelper.Instance.DevAdminUserName;
                        password = ConfigurationHelper.Instance.DevAdminPassword;
                        otherUsername = ConfigurationHelper.Instance.ProdAdminUserName;
                        otherPassword = ConfigurationHelper.Instance.ProdAdminPassword;
                    }
                    if (!WebSecurity.UserExists(username))
                    {
                        WebSecurity.CreateUserAndAccount(username, password);
                    }
                    if (!Roles.GetRolesForUser(username).Contains("Administrator"))//.GetRolesForUser(username).Contains("Administrator"))
                    {
                        Roles.AddUsersToRoles(new[] { username }, new[] { "Administrator" });
                    }
                    //((SimpleMembershipProvider)Membership.Provider).DeleteAccount(otherUsername);
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException("The ASP.NET Simple Membership database could not be initialized. For more information, please see http://go.microsoft.com/fwlink/?LinkId=256588", ex);
                }
            }
        }
    }
}
using System.Linq;

namespace Authentication.Models
{
    internal class UserRepository
    {
        public bool IsValidUser(string username)
        {
            using (var context = new UsersContext())
            {
                UserProfile userProfile = context.UserProfiles.Include("WebApplication").FirstOrDefault(u => u.UserName == username);
                if (userProfile == null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
    }
}

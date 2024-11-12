using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using userinfo.dal.Models;

namespace userinfo.dal.Repositories
{
    public class UserRepository : IUserRepository
    {
        public Guid GetUserId(string userName)
        {
            if (string.IsNullOrEmpty(userName)) //TODO: temp
                userName = "admin";

            using(var da = new iDestnEntities())
            {
                var user = da.Users.Where(x => x.UserName == userName).FirstOrDefault();
                if(user != null)
                    return user.UserId;
            }

            throw new SecurityException("Username not found.");
        }

    

    }
}

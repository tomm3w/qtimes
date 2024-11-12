using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace userinfo.dal.Repositories
{
    public interface IUserRepository
    {
        Guid GetUserId(string userName);
    }
}

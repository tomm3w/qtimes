using QTimes.core.dal.Models;
using System;

namespace QTimes.core.dal.Repositories
{
    public interface IUserInfoRepo
    {
        UserInfo RegisterReservationAccount(Guid userId, string businessName, string fullName, string email, string address, string cityTown,
            string state, string zip);
    }
}

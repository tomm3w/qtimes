using iVeew.common.dal;
using QTimes.core.dal.Enums;
using QTimes.core.dal.Models;
using System;

namespace QTimes.core.dal.Repositories
{
    public class UserInfoRepo : GenericRepository<QTimesContext, UserInfo>, IUserInfoRepo

    {
        public UserInfo RegisterReservationAccount(Guid userId, string businessName, string fullName,string email, string address, string cityTown,
            string state,string zip)
        {
            UserInfo usr = null;
            using (QTimesContext context = new QTimesContext())
            {
                ReservationBusiness res = new ReservationBusiness();
                res.BusinessName = businessName;
                res.FullName =fullName;
                res.Email = email;
                res.Address = address;
                res.City = cityTown;
                res.State = state;
                res.Zip = zip;
                res.Id = Guid.NewGuid();

                context.ReservationBusiness.Add(res);

                BusinessDetail businessDetail = new BusinessDetail();
                businessDetail.Id = Guid.NewGuid();
                businessDetail.ReservationBusinessId = res.Id;
                businessDetail.BusinessTypeId = (short)BusinessTypeEnum.Generic;
                businessDetail.BusinessName = businessName;
                context.BusinessDetail.Add(businessDetail);

                UserInfo ui = new UserInfo();
                ui.UserId = userId;
                ui.ReservationBusinessId = res.Id;
                ui.IsActive = true;
                ui.IsDeleted = false;
                ui.IsConfirmed = false;
                ui.ConfirmationToken = Guid.NewGuid();
                context.UserInfo.Add(ui);

                context.SaveChanges();

            }
            return usr;
        }
    }
}

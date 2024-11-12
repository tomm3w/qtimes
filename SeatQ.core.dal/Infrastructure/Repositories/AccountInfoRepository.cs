using Core.Exceptions;
using Core.Helpers;
using SeatQ.core.dal.Enums;
using SeatQ.core.dal.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SeatQ.core.dal.Infrastructure.Repositories
{
    public class AccountInfoRepository : IAccountInfoRepository
    {
        public RestaurantChain GetRestaurantInfo(RestaurantChainIdModel model)
        {
            RestaurantChain rc = null;
            using (SeatQEntities context = new SeatQEntities())
            {
                rc = context.RestaurantChains
                    .Include("Restaurant")
                    .Include("RestaurantClosedDays")
                    .Include("RestaurantTables.TableType")
                    .FirstOrDefault(r => r.RestaurantChainId == model.RestaurantChainId);
            }

            return rc;
        }

        public string GetRestaurantNumber(int RestaurantChainId)
        {
            RestaurantChain rc = null;
            using (SeatQEntities context = new SeatQEntities())
            {
                rc = context.RestaurantChains
                    .FirstOrDefault(r => r.RestaurantChainId == RestaurantChainId);
            }

            if (rc != null)
                return rc.RestaurantNumber == null ? "" : rc.RestaurantNumber.ToString();
            else
                return string.Empty;

        }

        public RestaurantChain UpdateRestaurantInfo(RestaurantInfoModel model)
        {
            RestaurantChain rc = null;
            using (SeatQEntities context = new SeatQEntities())
            {

                RestaurantChain resChn = context.RestaurantChains.FirstOrDefault(c => c.RestaurantChainId == model.RestaurantChainId && c.RestaurantId == model.RestaurantId);

                if (resChn != null)
                {
                    resChn.FullName = model.FullName;
                    resChn.Email = model.Email;
                    resChn.Address1 = model.Address1;
                    resChn.Address2 = model.Address2;
                    resChn.City = model.CityTown;
                    resChn.State = model.State;
                    resChn.Zip = model.Zip;
                    resChn.Phone = model.Phone;
                    resChn.RestaurantNumber = model.RestaurantNumber;
                    //resChn.AverageWaitTime = model.AverageWaitTime;
                    resChn.ModifiedDate = DateTime.UtcNow;
                    context.SaveChanges();

                    Restaurant res = context.Restaurants.FirstOrDefault(r => r.RestaurantId == model.RestaurantId);

                    if (res != null)
                    {
                        res.BusinessName = model.BusinessName;

                        string logoPath = string.Empty;
                        if (model.Image != null)
                        {
                            logoPath = ImageHelper.SaveImage(model.Image);
                            res.LogoPath = logoPath;
                        }

                        context.SaveChanges();
                    }

                }

                rc = context.RestaurantChains
                    .Include("Restaurant")
                    .FirstOrDefault(r => r.RestaurantChainId == model.RestaurantChainId && r.RestaurantId == model.RestaurantId);
            }

            return rc;
        }

        public UserProfile UpdateAccountInfo(AccountInfoModel model)
        {
            UserProfile usr = null;
            using (SeatQEntities context = new SeatQEntities())
            {
                usr = context.UserProfiles.FirstOrDefault(u => u.UserId == model.UserId);

                if (usr != null)
                {

                    //context.UpdateUserEmail(model.UserId, model.Email);
                    if (model.NewPassword != null)
                    {
                        if (Authentication.Web.Security.ValidateUser(model.UserName, model.CurrentPassword))
                        {
                            var userForUpdate = System.Web.Security.Membership.GetUser(model.UserName);
                            userForUpdate.ChangePassword(userForUpdate.ResetPassword(), model.NewPassword);
                            System.Web.Security.Membership.UpdateUser(userForUpdate);
                            //bool ok = Authentication.Web.Security.ChangePassword(model.UserName, model.CurrentPassword, model.NewPassword);
                        }
                        else
                        {
                            throw new CoreException { ErrorCode = ErrorCode.ErrorCodeInvalidPassword, ErrorType = ErrorType.authentication, ErrorMessage = @"Current password does not match" };
                        }
                    }

                    UserInfo usrInfo = context.UserInfoes.FirstOrDefault(u => u.UserId == model.UserId);
                    if (usrInfo != null)
                    {
                        usrInfo.isActive = model.IsActive;
                        context.SaveChanges();
                    }

                }
            }

            return usr;
        }

        public UserInfo DeleteAccountInfo(AccountInfoModel model)
        {
            UserInfo usr = null;
            using (SeatQEntities context = new SeatQEntities())
            {
                usr = context.UserInfoes.FirstOrDefault(u => u.UserId == model.UserId);

                if (usr != null)
                {
                    usr.isDeleted = true;
                    context.SaveChanges();

                }
            }

            return usr;
        }

        public UserProfile AddHostess(HostessModel model)
        {
            UserProfile usr = null;

            using (SeatQEntities context = new SeatQEntities())
            {
                Authentication.Web.Security.CreateUserAndAccount(model.UserName, model.Password, new { Email = model.Email });
                Authentication.Web.Security.AddUsersToRoles(model.UserName, "User");//hostess


                Guid userId = Authentication.Web.Security.CurrentUserId;
                UserInfo ui = context.UserInfoes.Create();
                ui.UserId = userId;
                ui.RestaurantChainId = model.RestaurantChainId;
                ui.isActive = true;
                ui.isDeleted = false;
                context.UserInfoes.Add(ui);
                context.SaveChanges();

                usr = context.UserProfiles.FirstOrDefault(u => u.Email == model.Email);

            }

            return usr;
        }

        public List<GetHostess_Result> GetHostess(int RestaurantChainId)
        {
            List<GetHostess_Result> usr = null;
            using (SeatQEntities context = new SeatQEntities())
            {
                usr = context.GetHostess(RestaurantChainId).OrderBy(x => x.StaffTypeId).ThenBy(x => x.UserName).ToList();
            }

            return usr;
        }

        public UserProfile GetUserById(Guid UserId)
        {
            UserProfile usr = null;
            using (SeatQEntities context = new SeatQEntities())
            {
                usr = context.UserProfiles.FirstOrDefault(u => u.UserId == UserId);
            }

            return usr;
        }

        public UserInfo UpdateHostess(HostessModel model)
        {
            UserInfo user = null;
            using (SeatQEntities context = new SeatQEntities())
            {
                user = (from u in context.UserInfoes
                        where u.UserId == model.UserId
                        select u).FirstOrDefault();

                if (user != null)
                {
                    if (!string.IsNullOrEmpty(model.Password))
                    {
                        String passwordResetToken = Authentication.Web.Security.GeneratePasswordResetToken(model.UserName);
                        Authentication.Web.Security.ResetPassword(passwordResetToken, model.Password);
                    }

                    //user.Email = model.Email;
                    user.isActive = model.IsActive;
                    context.SaveChanges();

                    context.UpdateUserEmail(model.UserId, model.Email);
                }
            }

            return user;
        }

        public void UpdateUserEmail(Guid userid, string email)
        {
            using (SeatQEntities context = new SeatQEntities())
            {
                context.UpdateUserEmail(userid, email);
            }
        }

        public Restaurant UpdateImage(int RestaurantId, string LogoPath)
        {
            Restaurant r = null;
            using (SeatQEntities context = new SeatQEntities())
            {

                r = context.Restaurants.FirstOrDefault(x => x.RestaurantId == RestaurantId);

                if (r.LogoPath != null)
                    ImageHelper.DeleteImageWithRelativePath(r.LogoPath);

                r.LogoPath = LogoPath;
                context.SaveChanges();
            }
            return r;
        }

        public RestaurantChain UpdateTableLayout(int RestaurantChainId, string TableLayoutPath)
        {
            RestaurantChain r = null;
            using (SeatQEntities context = new SeatQEntities())
            {

                r = context.RestaurantChains.FirstOrDefault(x => x.RestaurantChainId == RestaurantChainId);

                if (r.TableLayoutPath != null)
                    ImageHelper.DeleteImageWithRelativePath(r.TableLayoutPath);

                r.TableLayoutPath = TableLayoutPath;
                context.SaveChanges();
            }
            return r;
        }

        public Restaurant GetRestaurantByBusinessId(int businessId)
        {
            Restaurant usr = null;
            using (SeatQEntities context = new SeatQEntities())
            {
                usr = context.UsersInRestaurants.FirstOrDefault(u => u.BusinessId == businessId).RestaurantChain.Restaurant;
            }

            return usr;
        }

        #region User

        public UserProfile GetUserByUserName(string Username)
        {
            UserProfile user = null;
            using (SeatQEntities context = new SeatQEntities())
            {
                user = context.UserProfiles
                    //.Include("webpages_Roles")
                    //.Include("RestaurantChain")
                    //.Include("RestaurantChain.Restaurant")
                    .FirstOrDefault(u => u.UserName == Username);
            }
            return user;
        }

        public UserProfile RegisterAccount(SignupModel model)
        {
            UserProfile usr = null;
            var user = Authentication.Web.Security.CreateUserAndRole(model.Username, model.Password, model.Email);

            if (user != null)
            {
                using (SeatQEntities context = new SeatQEntities())
                {
                    Restaurant res = new Restaurant();
                    res.BusinessName = model.BusinessName;
                    res.SignUpDate = DateTime.UtcNow;
                    res.PlanId = 1;//TODO:add user selected plan type
                    res.ConfirmationToken = Guid.NewGuid();
                    res.isConfirmed = false; //Set to false

                    RestaurantChain resChain = new RestaurantChain();
                    resChain = context.RestaurantChains.Create();
                    resChain.FullName = model.FullName;
                    resChain.Email = model.Email;
                    resChain.Address1 = model.Address1;
                    resChain.Address2 = model.Address2;
                    resChain.City = model.CityTown;
                    resChain.State = model.State;
                    resChain.Zip = model.Zip;
                    resChain.Phone = model.Phone;
                    resChain.IsActive = true;//Set to false

                    res.RestaurantChains.Add(resChain);
                    context.Restaurants.Add(res);

                    Guid userId = Authentication.Web.Security.CurrentUserIdByName(user.UserName);
                    UserInfo ui = context.UserInfoes.Create();
                    ui.UserId = userId;
                    ui.RestaurantChainId = resChain.RestaurantChainId;
                    ui.isActive = true;
                    ui.isDeleted = false;
                    ui.StaffTypeId = (short)StaffTypeEnum.Admin;
                    context.UserInfoes.Add(ui);

                    UsersInRestaurant usrInRes = context.UsersInRestaurants.Create();
                    usrInRes.UserId = userId;
                    usrInRes.RestaurantChainId = resChain.RestaurantChainId;
                    context.UsersInRestaurants.Add(usrInRes);

                    context.SaveChanges();

                    usr = context.UserProfiles
                        .FirstOrDefault(u => u.UserId == userId);
                }

            }
            return usr;
        }

        public UserProfile RegisterReservationAccount(SignupModel model)
        {
            UserProfile usr = null;
            var user = Authentication.Web.Security.CreateUserAndRole(model.Username, model.Password, model.Email);

            if (user != null)
            {
                using (SeatQEntities context = new SeatQEntities())
                {
                    ReservationBusiness res = context.ReservationBusinesses.Create();
                    res.BusinessName = model.BusinessName;
                    res.FullName = model.FullName;
                    res.Email = model.Email;
                    res.Address = model.Address1;
                    res.City = model.CityTown;
                    res.State = model.State;
                    res.Zip = model.Zip;
                    res.Id = Guid.NewGuid();

                    context.ReservationBusinesses.Add(res);

                    BusinessDetail businessDetail = context.BusinessDetails.Create();
                    businessDetail.Id = Guid.NewGuid();
                    businessDetail.ReservationBusinessId = res.Id;
                    businessDetail.BusinessTypeId = (short)BusinessTypeEnum.Default;
                    businessDetail.BusinessName = model.BusinessName;
                    context.BusinessDetails.Add(businessDetail);

                    Guid userId = Authentication.Web.Security.CurrentUserIdByName(user.UserName);
                    UserInfo ui = context.UserInfoes.Create();
                    ui.UserId = userId;
                    ui.ReservationBusinessId = res.Id;
                    ui.isActive = true;
                    ui.isDeleted = false;
                    ui.StaffTypeId = (short)StaffTypeEnum.Admin;
                    ui.IsConfirmed = false;
                    ui.ConfirmationToken = Guid.NewGuid();
                    context.UserInfoes.Add(ui);

                    context.SaveChanges();

                    usr = usr = context.UserProfiles
                        .FirstOrDefault(u => u.UserId == userId);

                    usr.Email = model.Email;
                }

            }
            return usr;
        }

        public bool ConfirmReservationRegistration(Guid? confirmationToken)
        {
            bool retVal = false;
            using (SeatQEntities context = new SeatQEntities())
            {
                UserInfo res = context.UserInfoes.FirstOrDefault(r => r.ConfirmationToken == confirmationToken && r.IsConfirmed == false);

                if (res != null)
                {
                    res.IsConfirmed = true;
                    context.SaveChanges();
                    retVal = true;
                }
            }

            return retVal;
        }

        public bool ConfirmRegistration(Guid? confirmationToken)
        {
            bool retVal = false;
            using (SeatQEntities context = new SeatQEntities())
            {
                Restaurant res = context.Restaurants.FirstOrDefault(r => r.ConfirmationToken == confirmationToken && r.isConfirmed == false);

                if (res != null)
                {
                    res.isConfirmed = true;
                    res.ConfirmedDate = DateTime.UtcNow;
                    context.SaveChanges();
                    retVal = true;
                }
            }

            return retVal;
        }

        public UserInfo GetUserInfoByUserId(Guid UserId)
        {
            UserInfo user = null;
            using (SeatQEntities context = new SeatQEntities())
            {
                user = context.UserInfoes
                    .FirstOrDefault(u => u.UserId == UserId);
            }
            return user;
        }

        public UserProfile GetUserByUserId(Guid UserId)
        {
            UserProfile user = null;
            using (SeatQEntities context = new SeatQEntities())
            {
                user = context.UserProfiles
                    .FirstOrDefault(u => u.UserId == UserId);
            }
            return user;
        }

        public void UpdateLastAccess(Guid UserId)
        {

            using (SeatQEntities context = new SeatQEntities())
            {
                UserInfo usr = context.UserInfoes.FirstOrDefault(u => u.UserId == UserId);
                if (usr != null)
                {
                    usr.LastAccessDateTime = DateTime.UtcNow;
                    context.SaveChanges();
                }
            }
        }

        public bool IsPasswordChanged(Guid UserId)
        {
            bool retVal = false;
            using (SeatQEntities context = new SeatQEntities())
            {
                UserInfo usr = context.UserInfoes.FirstOrDefault(u => u.UserId == UserId);
                if (usr != null)
                    retVal = usr.IsPasswordChanged == null ? false : (bool)usr.IsPasswordChanged;
            }
            return retVal;
        }

        public bool UpdatePasswordChanged(Guid UserId)
        {
            bool retVal = false;
            using (SeatQEntities context = new SeatQEntities())
            {
                UserInfo usr = context.UserInfoes.FirstOrDefault(u => u.UserId == UserId);
                if (usr != null)
                {
                    usr.IsPasswordChanged = false;
                    context.SaveChanges();
                }
            }
            return retVal;
        }

        public UserInfo GetUserInfo(Guid UserId)
        {
            UserInfo usr = null;
            using (SeatQEntities context = new SeatQEntities())
            {
                usr = context.UserInfoes
                    .Include("RestaurantChain.Restaurant")
                    .Include("UsersInRestaurants")
                    .FirstOrDefault(u => u.UserId == UserId);
            }
            return usr;
        }

        public UserInfo RegisterUserInfo(Guid UserId)
        {
            UserInfo ui = null;
            using (SeatQEntities context = new SeatQEntities())
            {
                ui = context.UserInfoes.Create();
                ui.UserId = UserId;
                ui.RestaurantChainId = null;
                ui.isActive = true;
                ui.isDeleted = false;
                ui.LastAccessDateTime = DateTime.UtcNow;
                ui.IsPasswordChanged = false;
                context.UserInfoes.Add(ui);
                context.SaveChanges();
            }
            return ui;
        }

        #endregion

        #region Preferences

        public RestaurantChain UpdatePreferences(PreferencesModel model)
        {
            RestaurantChain res = null;
            using (SeatQEntities context = new SeatQEntities())
            {
                res = context.RestaurantChains.FirstOrDefault(u => u.RestaurantChainId == model.RestaurantChainId);

                if (res != null)
                {
                    res.OpeningHour = model.OpeningHour;
                    res.ClosingHour = model.ClosingHour;
                    res.WaittimeInterval = model.WaittimeInterval;
                }

                List<RestaurantClosedDay> day = context.RestaurantClosedDays.Where(x => x.RestaurantChainId == model.RestaurantChainId).ToList();
                if (day.Count > 0)
                {
                    context.RestaurantClosedDays.RemoveRange(day);
                }

                foreach (int val in model.ClosedDay)
                {
                    res.RestaurantClosedDays.Add(new RestaurantClosedDay { Days = val });
                }


                context.SaveChanges();
            }

            return res;
        }
        #endregion

        #region Tables

        public List<TableType> GetTableType()
        {
            using (SeatQEntities context = new SeatQEntities())
            {
                return context.TableTypes.ToList();
            }

        }

        public RestaurantTable GetTable(int TableId)
        {
            RestaurantTable rc = null;
            using (SeatQEntities context = new SeatQEntities())
            {
                rc = context.RestaurantTables
                    .Include("TableType")
                    .FirstOrDefault(r => r.TableId == TableId);
            }

            return rc;
        }
        public List<RestaurantTable> GetRestaurantTables(int RestaurantChainId)
        {
            List<RestaurantTable> rc = null;
            using (SeatQEntities context = new SeatQEntities())
            {
                rc = context.RestaurantTables
                    .Include("TableType")
                    .Where(r => r.RestaurantChainId == RestaurantChainId).ToList();
            }

            return rc;
        }

        public RestaurantTable GetRestaurantTableInfo(int TableId)
        {
            RestaurantTable rc = null;
            using (SeatQEntities context = new SeatQEntities())
            {
                rc = context.RestaurantTables
                    .Include("TableType")
                    .FirstOrDefault(r => r.TableId == TableId);
            }

            return rc;
        }

        public RestaurantTable AddRestaurantTable(RestaurantTableModel model)
        {
            RestaurantTable rt = null;
            using (SeatQEntities context = new SeatQEntities())
            {

                rt = context.RestaurantTables.Create();
                rt.RestaurantChainId = model.RestaurantChainId;
                rt.TableTypeId = model.TableTypeId;
                rt.TableNumber = model.TableNumber;
                rt.IsAvailable = model.IsAvailable ?? false;
                context.RestaurantTables.Add(rt);
                context.SaveChanges();

            }

            return rt;
        }


        public RestaurantTable UpdateRestaurantTable(RestaurantTableModel model)
        {
            RestaurantTable rt = null;
            using (SeatQEntities context = new SeatQEntities())
            {

                rt = context.RestaurantTables.FirstOrDefault(x => x.TableId == model.TableId);
                if (rt != null)
                {
                    rt.RestaurantChainId = model.RestaurantChainId;
                    rt.TableTypeId = model.TableTypeId;
                    rt.TableNumber = model.TableNumber;
                    rt.IsAvailable = model.IsAvailable ?? false;
                    context.SaveChanges();
                }
            }

            return rt;
        }
        #endregion
    }
}
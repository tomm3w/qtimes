using SeatQ.core.dal.Models;
using System;
using System.Collections.Generic;

namespace SeatQ.core.dal.Infrastructure.Repositories
{
    public interface IAccountInfoRepository
    {
        RestaurantChain GetRestaurantInfo(RestaurantChainIdModel RestaurantChainId);
        string GetRestaurantNumber(int RestaurantChainId);
        RestaurantChain UpdateRestaurantInfo(RestaurantInfoModel model);
        UserProfile UpdateAccountInfo(AccountInfoModel model);
        UserInfo DeleteAccountInfo(AccountInfoModel model);
        UserProfile AddHostess(HostessModel model);
        List<GetHostess_Result> GetHostess(int model);
        UserProfile GetUserById(Guid UserId);
        UserInfo UpdateHostess(HostessModel model);

        RestaurantChain UpdateTableLayout(int RestaurantChainId, string TableLayoutPath);
        Restaurant UpdateImage(int RestaurantId, string LogoPath);

        void UpdateUserEmail(Guid userid,string email);
        #region User
        UserProfile GetUserByUserName(string Username);
        UserProfile RegisterAccount(SignupModel model);
        UserProfile RegisterReservationAccount(SignupModel model);
        bool ConfirmRegistration(Guid? confirmationToken);
        bool ConfirmReservationRegistration(Guid? confirmationToken);
        UserProfile GetUserByUserId(Guid UserId);
        UserInfo GetUserInfoByUserId(Guid UserId);
        void UpdateLastAccess(Guid UserId);
        bool IsPasswordChanged(Guid UserId);
        bool UpdatePasswordChanged(Guid UserId);
        UserInfo GetUserInfo(Guid UserId);
        UserInfo RegisterUserInfo(Guid UserId);

        RestaurantChain UpdatePreferences(PreferencesModel model);
        RestaurantTable GetTable(int TableId);
        List<RestaurantTable> GetRestaurantTables(int RestaurantChainId);
        RestaurantTable GetRestaurantTableInfo(int TableId);
        RestaurantTable AddRestaurantTable(RestaurantTableModel model);
        RestaurantTable UpdateRestaurantTable(RestaurantTableModel model);
        List<TableType> GetTableType();
        #endregion
    }
}

using HastyAPI.Nexmo;
using SeatQ.core.dal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeatQ.core.dal.Infrastructure.Repositories
{
    public interface IWaitListRepository
    {
        WaitList AddWaitList(AddWaitListModel model);
        WaitList UpdateWaitList(AddWaitListModel model);
        WaitListModel GetWaitList(PagingModel model, int RestaurantChainId);
        ReturnGuestModel GetReturnGuest(PagingModel model, MetricsModel where);
        WaitList GetWaitlistById(int waitList);
        WaitList SendTextMessage(int waitList);
        WaitList Confirmed(int waitList);
        WaitList Leave(int waitList);
        WaitList Seated(int waitList);
        GetWaitListFromReply_Result GetWaitListToConfirmedByMobileNumber(string RestaurantNumber, string MobileNumber, DateTime ReplyDateTime);
        List<GuestType> GetGuestType();
        int GetAverageWaitTime(int RestaurantChainId);
        WaitList SendMessageToGuest(SendMessageToGuestModel model, SubmissionResponse response, string from, string to, string msg);
        GetTableSummary_Result GetTableSummary(int RestaurantChainId);
        LoyaltyModel GetLoyalty(PagingModel model, int RestaurantChainId);
        WaitList SendLoyaltyMessageToGuest(SendLoyaltyMessageToGuestModel model, SubmissionResponse response, string from, string to, string msg);
        void SaveVisitMessage(WaitList model, SubmissionResponse response, string from, string to, string msg);
        List<GetTablesWithSeating_Result> GetTablesWithSeating(int RestaurantChainId, string userId);
        bool CloseTable(int RestaurantChainId, int TableId);
    }
}

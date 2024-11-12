//using HastyAPI.Nexmo;
//using SeatQ.core.dal.Models;
//using System.Collections.Generic;

//namespace QTimes.core.dal.Infrastructure.Repositories
//{
//    public interface IMessageRepository
//    {
//        List<ReadyMessage> GetReadyMessage(RestaurantChainIdModel model);
//        ReadyMessage InsertReadyMessage(RestaurantChainIdModel model);
//        List<VisitMessage> GetVisitMessage(RestaurantChainIdModel model);
//        List<ReadyMessage> UpdateReadyMessage(List<ReadyMessage> model, int RestaurantChainId);
//        List<VisitMessage> UpdateVisitMessag(List<VisitMessage> model, int RestaurantChainId);
//        void SaveResponse(WaitList wl, SubmissionResponse res, string from, string msg);
//        void SaveReply(InBoundMessageModel model, long? WaitListId);
//        void SaveVisitResponse(GetReturnGuest_Result wl, SubmissionResponse res, string from, string msg);
//        void SaveDeliveryStatus(DeliveryReceiptModel model);
//        string GetReadyMessage(int RestaurantChainId);
//        string GetVisitMessage(GetReturnGuest_Result model);
//        string GetVisitMessage(int RestaurantChainId, int NoOfReturn);
//    }
//}

//using HastyAPI.Nexmo;
//using SeatQ.core.dal.Models;
//using System;
//using System.Collections.Generic;
//using System.Data.Entity.Core.Objects;
//using System.Linq;

//namespace QTimes.core.dal.Infrastructure.Repositories
//{
//    public class WaitListRepository : IWaitListRepository
//    {
//        public WaitList AddWaitList(AddWaitListModel model)
//        {
//            WaitList wl = null;
//            using (SeatQEntities context = new SeatQEntities())
//            {
//                GuestInfo gi = context.GuestInfoes.FirstOrDefault(g => /*g.GuestName == model.GuestName &&*/ g.MobileNumber == model.MobileNumber && g.RestaurantChainId == model.RestaurantChainId);

//                if (gi == null)
//                {
//                    gi = context.GuestInfoes.Create();
//                    gi.GuestName = model.GuestName;
//                    gi.MobileNumber = model.MobileNumber;
//                    gi.NoOfReturn = 0;
//                    gi.RestaurantChainId = model.RestaurantChainId;
//                    context.GuestInfoes.Add(gi);
//                    context.SaveChanges();
//                }

//                wl = context.WaitLists.Create();

//                wl.RestaurantChainId = model.RestaurantChainId;
//                wl.GuestId = gi.GuestId;
//                wl.GroupSize = model.GroupSize;
//                wl.GuestTypeId = model.GuestTypeId;
//                wl.EnteredBy = model.EnteredBy;
//                wl.EnteredDateTime = DateTime.UtcNow;
//                wl.EstimatedDateTime = Convert.ToDateTime(model.EstimatedDateTime).ToUniversalTime();
//                wl.ActualDateTime = Convert.ToDateTime(model.ActualDateTime).ToUniversalTime();
//                wl.IsMessageSent = false;
//                wl.IsSeated = false;
//                wl.IsLeft = false;
//                wl.TableId = model.TableId;
//                wl.RoomNumber = model.RoomNumber;
//                wl.GuestName = model.GuestName;

//                context.WaitLists.Add(wl);
//                context.SaveChanges();
//            }

//            return wl;
//        }

//        public WaitList UpdateWaitList(AddWaitListModel model)
//        {
//            WaitList wl = null;
//            using (SeatQEntities context = new SeatQEntities())
//            {
//                wl = context.WaitLists.FirstOrDefault(w => w.WaitListId == model.WaitListId);
//                if (wl != null)
//                {
//                    wl.GroupSize = model.GroupSize;
//                    wl.GuestTypeId = model.GuestTypeId;
//                    wl.EstimatedDateTime = Convert.ToDateTime(model.EstimatedDateTime).ToUniversalTime();
//                    wl.ActualDateTime = Convert.ToDateTime(model.ActualDateTime).ToUniversalTime();
//                    wl.TableId = model.TableId;
//                    wl.RoomNumber = model.RoomNumber;
//                    wl.GuestName = model.GuestName;
//                    context.SaveChanges();

//                    GuestInfo gi = context.GuestInfoes.FirstOrDefault(g => g.GuestId == wl.GuestId);
//                    if (gi != null)
//                    {
//                        gi.GuestName = model.GuestName;
//                        gi.MobileNumber = model.MobileNumber;
//                        context.SaveChanges();
//                    }
//                }
//            }

//            return wl;
//        }

//        /*public WaitListModel GetWaitList(PagingModel model, int RestaurantChainId)
//        {
//            WaitListModel retVal = null;
//            using (SeatQEntities context = new SeatQEntities())
//            {
//                var businesses = context.WaitLists
//                    .Where(w => w.RestaurantChainId == RestaurantChainId && w.IsSeated == false && (w.MessageReply == 1 || w.MessageReply == null))
//                    .OrderByDescending(w => w.EnteredDateTime)
//                    .AsQueryable();

//                model.TotalData = businesses.Count();
//                if (model.Page != null && model.PageSize != null)
//                {
//                    businesses = businesses.Skip(((int)model.Page - 1) * (int)model.PageSize).Take((int)model.PageSize);
//                    model.TotalPages = (int)Math.Ceiling((decimal)model.TotalData / (decimal)model.PageSize);
//                }
//                else
//                {
//                    model.PageSize = 10;
//                    model.Page = 1;
//                    model.TotalPages = (int)Math.Ceiling((decimal)model.TotalData / (decimal)model.PageSize);
//                }

//                retVal = new WaitListModel();
//                retVal.Businesses = businesses.ToList();
//                retVal.Paging = model;
//            }

//            return retVal;
//        }*/

//        public WaitListModel GetWaitList(PagingModel model, int RestaurantChainId)
//        {
//            WaitListModel retVal = null;
//            using (SeatQEntities context = new SeatQEntities())
//            {
//                var TotalData = new ObjectParameter("TotalCount", typeof(int));

//                int startRowNumber = (((int)model.Page - 1) * (int)model.PageSize) + 1;
//                List<GetWaitList_Result> businesses = context.GetWaitList(RestaurantChainId, startRowNumber, model.PageSize, TotalData).ToList();

//                model.TotalData = (Nullable<int>)TotalData.Value;

//                if (model.Page != null && model.PageSize != null)
//                {
//                    //businesses = businesses.Skip(((int)model.Page - 1) * (int)model.PageSize).Take((int)model.PageSize);
//                    model.TotalPages = (int)Math.Ceiling((decimal)model.TotalData / (decimal)model.PageSize);
//                }
//                else
//                {
//                    model.PageSize = 10;
//                    model.Page = 1;
//                    model.TotalPages = (int)Math.Ceiling((decimal)model.TotalData / (decimal)model.PageSize);
//                }

//                retVal = new WaitListModel();
//                retVal.Businesses = businesses;
//                retVal.Paging = model;
//            }

//            return retVal;
//        }


//        public ReturnGuestModel GetReturnGuest(PagingModel model, MetricsModel where)
//        {
//            ReturnGuestModel retVal = null;

//            using (SeatQEntities context = new SeatQEntities())
//            {
//                var TotalData = new ObjectParameter("TotalCount", typeof(int));

//                int startRowNumber = (((int)model.Page - 1) * (int)model.PageSize) + 1;
//                List<GetReturnGuest_Result> businesses = context.GetReturnGuest(where.RestaurantChainId, where.MetricsType, where.StartDate, where.EndDate, startRowNumber, model.PageSize, TotalData).ToList();

//                model.TotalData = (Nullable<int>)TotalData.Value;

//                if (model.Page != null && model.PageSize != null)
//                {
//                    //businesses = businesses.Skip(((int)model.Page - 1) * (int)model.PageSize).Take((int)model.PageSize);
//                    model.TotalPages = (int)Math.Ceiling((decimal)model.TotalData / (decimal)model.PageSize);
//                }
//                else
//                {
//                    model.PageSize = 10;
//                    model.Page = 1;
//                    model.TotalPages = (int)Math.Ceiling((decimal)model.TotalData / (decimal)model.PageSize);
//                }

//                retVal = new ReturnGuestModel();
//                retVal.Businesses = businesses;
//                retVal.Paging = model;
//            }

//            return retVal;
//        }

//        public LoyaltyModel GetLoyalty(PagingModel model, int RestaurantChainId)
//        {
//            LoyaltyModel retVal = null;

//            using (SeatQEntities context = new SeatQEntities())
//            {
//                var TotalData = new ObjectParameter("TotalCount", typeof(int));

//                int startRowNumber = (((int)model.Page - 1) * (int)model.PageSize) + 1;
//                List<GetLoyalty_Result> businesses = context.GetLoyalty(RestaurantChainId, startRowNumber, model.PageSize, TotalData).ToList();

//                model.TotalData = (Nullable<int>)TotalData.Value;

//                if (model.Page != null && model.PageSize != null)
//                {
//                    //businesses = businesses.Skip(((int)model.Page - 1) * (int)model.PageSize).Take((int)model.PageSize);
//                    model.TotalPages = (int)Math.Ceiling((decimal)model.TotalData / (decimal)model.PageSize);
//                }
//                else
//                {
//                    model.PageSize = 10;
//                    model.Page = 1;
//                    model.TotalPages = (int)Math.Ceiling((decimal)model.TotalData / (decimal)model.PageSize);
//                }

//                retVal = new LoyaltyModel();
//                retVal.Businesses = businesses;
//                retVal.Paging = model;
//            }

//            return retVal;
//        }
//        public WaitList GetWaitlistById(int waitList)
//        {
//            WaitList wl = null;
//            using (SeatQEntities context = new SeatQEntities())
//            {
//                wl = context.WaitLists
//                    .Include("GuestInfo")
//                    .Include("RestaurantChain.Restaurant")
//                    .FirstOrDefault(w => w.WaitListId == waitList);
//            }

//            return wl;

//        }

//        public WaitList SendTextMessage(int waitList)
//        {
//            WaitList wl = null;
//            using (SeatQEntities context = new SeatQEntities())
//            {
//                wl = context.WaitLists.FirstOrDefault(w => w.WaitListId == waitList);
//                wl.IsMessageSent = true;
//                wl.MessageSentDateTime = DateTime.UtcNow;
//                context.SaveChanges();
//            }

//            return wl;

//        }

//        public WaitList SendMessageToGuest(SendMessageToGuestModel model, SubmissionResponse response, string from, string to, string msg)
//        {
//            WaitList wl = null;
//            using (SeatQEntities context = new SeatQEntities())
//            {
//                response.Messages.ForEach(m =>
//                {
//                    GuestMessage ms = context.GuestMessages.Create();
//                    ms.WaitListId = (int)model.WaitListId;
//                    ms.Message = msg;
//                    //ms.MessageFrom = from;
//                    ms.MessageTo = to;
//                    ms.StatusCode = (int)m.Status;
//                    ms.StatusMessage = m.Status.ToString();
//                    ms.MessageId = m.MessageID;
//                    ms.ErrorText = m.ErrorText;
//                    ms.ClientRef = m.ClientRef;
//                    ms.MessageDateTime = DateTime.UtcNow;
//                    context.GuestMessages.Add(ms);
//                    context.SaveChanges();
//                });
//            }

//            return wl;

//        }
//        public WaitList Confirmed(int waitList)
//        {
//            WaitList wl = null;
//            using (SeatQEntities context = new SeatQEntities())
//            {
//                wl = context.WaitLists.FirstOrDefault(w => w.WaitListId == waitList);
//                wl.MessageReply = 1; //1=confirmed
//                wl.MessageReplyRecivedDatetime = DateTime.UtcNow;
//                context.SaveChanges();
//            }

//            return wl;

//        }

//        public WaitList Leave(int waitList)
//        {
//            WaitList wl = null;
//            using (SeatQEntities context = new SeatQEntities())
//            {
//                wl = context.WaitLists.FirstOrDefault(w => w.WaitListId == waitList);
//                wl.MessageReply = 2; //1=leave
//                wl.MessageReplyRecivedDatetime = DateTime.UtcNow;
//                context.SaveChanges();
//            }

//            return wl;

//        }

//        public WaitList Seated(int waitList)
//        {
//            WaitList wl = null;
//            using (SeatQEntities context = new SeatQEntities())
//            {
//                wl = context.WaitLists.FirstOrDefault(w => w.WaitListId == waitList);
//                wl.IsSeated = true;
//                wl.SeatedDateTime = DateTime.UtcNow;
//                context.SaveChanges();

//                GuestInfo gi = context.GuestInfoes.FirstOrDefault(g => g.GuestId == wl.GuestId);
//                if (gi != null)
//                {
//                    gi.NoOfReturn = gi.NoOfReturn + 1;
//                    context.SaveChanges();
//                }
//            }

//            return wl;

//        }

//        public GetWaitListFromReply_Result GetWaitListToConfirmedByMobileNumber(string RestaurantNumber, string MobileNumber, DateTime ReplyDateTime)
//        {

//            GetWaitListFromReply_Result waitlist = null;
//            using (SeatQEntities context = new SeatQEntities())
//            {

//                //waitlist = context.WaitLists.FirstOrDefault(w => w.RestaurantChain.RestaurantNumber == RestaurantNumber && w.GuestInfo.MobileNumber == MobileNumber && w.MessageReply == null && w.IsMessageSent == true && w.IsSeated != true);
//                waitlist = context.GetWaitListFromReply(RestaurantNumber, MobileNumber, ReplyDateTime).FirstOrDefault();
//            }
//            return waitlist;
//        }

//        public GetWaitListFromGuestReply_Result GetWaitListForGuestMessage(string RestaurantNumber, string MobileNumber, DateTime ReplyDateTime)
//        {

//            GetWaitListFromGuestReply_Result waitlist = null;
//            using (SeatQEntities context = new SeatQEntities())
//            {

//                //waitlist = context.WaitLists.FirstOrDefault(w => w.RestaurantChain.RestaurantNumber == RestaurantNumber && w.GuestInfo.MobileNumber == MobileNumber && w.MessageReply == null && w.IsMessageSent == true && w.IsSeated != true);
//                waitlist = context.GetWaitListFromGuestReply(RestaurantNumber, MobileNumber, ReplyDateTime).FirstOrDefault();
//            }
//            return waitlist;
//        }

//        public List<GuestType> GetGuestType()
//        {
//            List<GuestType> gt = null;
//            using (SeatQEntities context = new SeatQEntities())
//            {
//                gt = context.GuestTypes.ToList();
//            }

//            return gt;
//        }

//        public int GetAverageWaitTime(int RestaurantChainId)
//        {
//            int mins;
//            using (SeatQEntities context = new SeatQEntities())
//            {
//                var AverageWaitTime = new ObjectParameter("AverageWaitTime", typeof(int));
//                context.GetAverageWaitTime(RestaurantChainId, AverageWaitTime);
//                mins = (int)AverageWaitTime.Value;
//            }
//            return mins;
//        }

//        public GetTableSummary_Result GetTableSummary(int RestaurantChainId)
//        {
//            GetTableSummary_Result retVal = null;
//            using (SeatQEntities context = new SeatQEntities())
//            {
//                retVal = context.GetTableSummary(RestaurantChainId).FirstOrDefault();
//            }

//            return retVal;
//        }

//        public WaitList SendLoyaltyMessageToGuest(SendLoyaltyMessageToGuestModel model, SubmissionResponse response, string from, string to, string msg)
//        {
//            WaitList wl = null;
//            using (SeatQEntities context = new SeatQEntities())
//            {
//                response.Messages.ForEach(m =>
//                {
//                    LoyaltyMessage ms = context.LoyaltyMessages.Create();
//                    ms.GuestId = (int)model.GuestId;
//                    ms.Message = msg;
//                    //ms.MessageFrom = from;
//                    ms.MessageTo = to;
//                    ms.StatusCode = (int)m.Status;
//                    ms.StatusMessage = m.Status.ToString();
//                    ms.MessageId = m.MessageID;
//                    ms.ErrorText = m.ErrorText;
//                    ms.ClientRef = m.ClientRef;
//                    ms.MessageDateTime = DateTime.UtcNow;
//                    context.LoyaltyMessages.Add(ms);
//                    context.SaveChanges();
//                });
//            }

//            return wl;

//        }

//        public void SaveVisitMessage(WaitList model, SubmissionResponse response, string from, string to, string msg)
//        {
//            using (SeatQEntities context = new SeatQEntities())
//            {

//                response.Messages.ForEach(m =>
//                {
//                    VisitMessageSent vms = context.VisitMessageSents.Create();
//                    vms.GuestId = model.GuestId;
//                    vms.RestaurantChainId = model.RestaurantChainId;
//                    vms.Visit = model.Visit;
//                    vms.MessageText = msg;
//                    vms.MessageFrom = from;
//                    vms.MessageTo = to;
//                    vms.StatusCode = (int)m.Status;
//                    vms.StatusMessage = m.Status.ToString();
//                    vms.MessageId = m.MessageID;
//                    vms.ErrorText = m.ErrorText;
//                    vms.ClientRef = m.ClientRef;
//                    vms.MessageSentDateTime = DateTime.UtcNow;
//                    context.VisitMessageSents.Add(vms);
//                    context.SaveChanges();
//                });
//            }
//        }
//        public bool IsVisitMessageExists(int RestaurantChainId, int NoOfReturn)
//        {
//            bool retVal = false;
//            using (SeatQEntities ctx = new SeatQEntities())
//            {
//                int v = ctx.VisitMessages.Count(x => x.RestaurantChainId == RestaurantChainId && x.Visit == NoOfReturn && x.IsEnabled == true && x.IsDeleted == false);
//                if (v > 0)
//                    retVal = true;
//            }
//            return retVal;
//        }

//        public List<GetTablesWithSeating_Result> GetTablesWithSeating(int RestaurantChainId, string userId)
//        {
//            List<GetTablesWithSeating_Result> retVal = null;
//            using (SeatQEntities ctx = new SeatQEntities())
//            {
//                retVal = ctx.GetTablesWithSeating(RestaurantChainId).ToList();
//            }
//            return retVal;
//        }

//        public bool CloseTable(int RestaurantChainId, int TableId)
//        {
            
//            using (SeatQEntities ctx = new SeatQEntities())
//            {
//                ctx.CloseTable(RestaurantChainId, TableId);
//            }
//            return true;
//        }
//    }
//}
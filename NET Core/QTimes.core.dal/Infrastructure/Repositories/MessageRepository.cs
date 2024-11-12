//using System;
//using System.Collections.Generic;
//using System.Linq;
//using HastyAPI.Nexmo;
//using SeatQ.core.dal.Models;

//namespace QTimes.core.dal.Infrastructure.Repositories
//{
//    public class MessageRepository : IMessageRepository
//    {
//        public List<ReadyMessage> GetReadyMessage(RestaurantChainIdModel model)
//        {
//            List<ReadyMessage> retVal = null;
//            using (SeatQEntities context = new SeatQEntities())
//            {
//                retVal = context.ReadyMessages.Where(m => m.RestaurantChainId == model.RestaurantChainId && m.IsDeleted == false).ToList();
//            }

//            return retVal;
//        }

//        public ReadyMessage InsertReadyMessage(RestaurantChainIdModel model)
//        {
//            ReadyMessage msg = null;
//            using (SeatQEntities context = new SeatQEntities())
//            {
//                msg = context.ReadyMessages.Create();
//                msg.RestaurantChainId = model.RestaurantChainId;
//                msg.ReadyMessage1 = "Your table is ready please check with the hostess.";
//                msg.IsEnabled = true;
//                msg.IsDeleted = false;
//                msg.ModifiedDate = DateTime.UtcNow;
//                context.ReadyMessages.Add(msg);
//                context.SaveChanges();
//            }
//            return msg;
//        }

//        public List<VisitMessage> GetVisitMessage(RestaurantChainIdModel model)
//        {
//            List<VisitMessage> retVal = null;
//            using (SeatQEntities context = new SeatQEntities())
//            {
//                retVal = context.VisitMessages.Where(m => m.RestaurantChainId == model.RestaurantChainId && m.IsDeleted == false).ToList();
//            }

//            return retVal;
//        }

//        public string GetVisitMessage(int RestaurantChainId, int NoOfReturn)
//        {
//            string retVal = "";
//            VisitMessage msg = null;
//            using (SeatQEntities context = new SeatQEntities())
//            {
//                msg = context.VisitMessages.FirstOrDefault(m => m.RestaurantChainId == RestaurantChainId && m.Visit == NoOfReturn && m.IsDeleted == false && m.IsEnabled == true);
//                if (msg == null)
//                    msg = context.VisitMessages.FirstOrDefault(m => m.RestaurantChainId == RestaurantChainId && m.Visit == 1 && m.IsDeleted == false && m.IsEnabled == true);

//                if (msg != null)
//                    retVal = msg.VisitMessage1;
//            }

//            return retVal;
//        }
//        public List<ReadyMessage> UpdateReadyMessage(List<ReadyMessage> model, int RestaurantChainId)
//        {
//            List<ReadyMessage> retVal = null;
//            if (model != null)
//            {
//                using (SeatQEntities context = new SeatQEntities())
//                {
//                    model.ForEach(m =>
//                        {
//                            ReadyMessage rm = context.ReadyMessages.FirstOrDefault(v => m.RestaurantChainId == m.RestaurantChainId && v.ReadyMessageId == m.ReadyMessageId);

//                            if (rm == null)
//                            {
//                                rm = context.ReadyMessages.Create();
//                                rm.RestaurantChainId = m.RestaurantChainId;
//                                rm.ReadyMessage1 = m.ReadyMessage1;
//                                rm.IsEnabled = m.IsEnabled;
//                                rm.IsDeleted = m.IsDeleted;
//                                rm.ModifiedDate = DateTime.UtcNow;

//                                context.ReadyMessages.Add(rm);
//                                context.SaveChanges();

//                            }
//                            else
//                            {
//                                rm.ReadyMessage1 = m.ReadyMessage1;
//                                rm.IsEnabled = m.IsEnabled;
//                                rm.IsDeleted = m.IsDeleted;
//                                rm.ModifiedDate = DateTime.UtcNow;
//                                context.SaveChanges();
//                            }
//                        });
//                }
//            }

//            using (SeatQEntities context = new SeatQEntities())
//            {
//                retVal = context.ReadyMessages.Where(r => r.RestaurantChainId == RestaurantChainId).ToList();
//            }
//            return retVal;
//        }

//        public List<VisitMessage> UpdateVisitMessag(List<VisitMessage> model, int RestaurantChainId)
//        {
//            List<VisitMessage> retVal = null;
//            if (model != null)
//            {
//                using (SeatQEntities context = new SeatQEntities())
//                {
//                    model.ForEach(m =>
//                        {
//                            VisitMessage vm = context.VisitMessages.FirstOrDefault(v => v.RestaurantChainId == m.RestaurantChainId && v.VisitMessageId == m.VisitMessageId);
//                            if (vm == null)
//                            {
//                                vm = context.VisitMessages.Create();
//                                vm.RestaurantChainId = m.RestaurantChainId;
//                                vm.Visit = m.Visit;
//                                vm.VisitMessage1 = m.VisitMessage1;
//                                vm.IsEnabled = m.IsEnabled;
//                                vm.IsDeleted = m.IsDeleted;
//                                vm.ModifiedDate = DateTime.UtcNow;
//                                context.VisitMessages.Add(vm);
//                                context.SaveChanges();
//                            }
//                            else
//                            {
//                                vm.Visit = m.Visit;
//                                vm.VisitMessage1 = m.VisitMessage1;
//                                vm.IsEnabled = m.IsEnabled;
//                                vm.IsDeleted = m.IsDeleted;
//                                vm.ModifiedDate = DateTime.UtcNow;
//                                context.SaveChanges();
//                            }
//                        });
//                }
//            }

//            using (SeatQEntities context = new SeatQEntities())
//            {
//                retVal = context.VisitMessages.Where(v => v.RestaurantChainId == RestaurantChainId).ToList();
//            }
//            return retVal;
//        }

//        public void SaveResponse(WaitList wl, SubmissionResponse res, string from, string msg)
//        {
//            using (SeatQEntities context = new SeatQEntities())
//            {
//                res.Messages.ForEach(m =>
//                    {
//                        MessageSent ms = context.MessageSents.Create();
//                        ms.WaitListId = (int)wl.WaitListId;
//                        ms.MessageType = "ReadyMessage";
//                        ms.MessageText = msg;
//                        ms.MessageFrom = from;
//                        ms.MessageTo = wl.GuestInfo.MobileNumber;
//                        ms.StatusCode = (int)m.Status;
//                        ms.StatusMessage = m.Status.ToString();
//                        ms.MessageId = m.MessageID;
//                        ms.ErrorText = m.ErrorText;
//                        ms.ClientRef = m.ClientRef;
//                        ms.MessageSentDateTime = DateTime.UtcNow;
//                        context.MessageSents.Add(ms);
//                        context.SaveChanges();
//                    });
//            }
//        }

//        public void SaveVisitResponse(GetReturnGuest_Result wl, SubmissionResponse res, string from, string msg)
//        {
//            using (SeatQEntities context = new SeatQEntities())
//            {
//                res.Messages.ForEach(m =>
//                {
//                    VisitMessageSent ms = context.VisitMessageSents.Create();
//                    ms.GuestId = (int)wl.GuestId;
//                    ms.RestaurantChainId = wl.RestaurantChainId;
//                    ms.Visit = wl.NoOfReturn;
//                    ms.MessageText = msg;
//                    ms.MessageFrom = from;
//                    ms.MessageTo = wl.MobileNumber;
//                    ms.StatusCode = (int)m.Status;
//                    ms.StatusMessage = m.Status.ToString();
//                    ms.MessageId = m.MessageID;
//                    ms.ErrorText = m.ErrorText;
//                    ms.ClientRef = m.ClientRef;
//                    ms.MessageSentDateTime = DateTime.UtcNow;
//                    context.VisitMessageSents.Add(ms);
//                    context.SaveChanges();
//                });
//            }
//        }

//        public void SaveReply(InBoundMessageModel model, long? WaitListId)
//        {
//            using (SeatQEntities context = new SeatQEntities())
//            {
//                MessageReply mr = context.MessageReplies.Create();
//                mr.WaitListId = WaitListId;
//                mr.type = model.type;
//                mr.text = model.text;
//                mr.MessageTo = model.to;
//                mr.msisdn = model.msisdn;
//                mr.networkcode = model.network_code;
//                mr.messageid = model.messageId;
//                mr.messagetimestamp = model.message_timestamp;
//                context.MessageReplies.Add(mr);
//                context.SaveChanges();
//            }
//        }

//        public void SaveGuestMessage(InBoundMessageModel model, long? WaitListId)
//        {
//            using (SeatQEntities context = new SeatQEntities())
//            {
//                GuestMessage mr = context.GuestMessages.Create();
//                mr.WaitListId = WaitListId;
//                mr.Message = model.text;
//                mr.MessageTo = model.to;
//                mr.MessageFrom = model.msisdn;
//                mr.MessageId = model.messageId;
//                mr.MessageDateTime = model.message_timestamp;
//                context.GuestMessages.Add(mr);
//                context.SaveChanges();
//            }
//        }

//        public void SaveDeliveryStatus(DeliveryReceiptModel model)
//        {
//            using (SeatQEntities context = new SeatQEntities())
//            {
//                DeliveryStatu ds = context.DeliveryStatus.Create();
//                ds.msisdn = model.msisdn;
//                ds.to = model.to;
//                ds.network_code = model.network_code;
//                ds.messageId = model.messageId;
//                ds.price = model.price;
//                ds.status = model.status;
//                ds.scts = model.scts;
//                ds.err_code = model.err_code;
//                ds.message_timestamp = model.message_timestamp;
//                ds.Project = "seatq";
//                context.DeliveryStatus.Add(ds);
//                context.SaveChanges();
//            }
//        }
//        public string GetReadyMessage(int RestaurantChainId)
//        {
//            string msg = null;
//            using (SeatQEntities context = new SeatQEntities())
//            {
//                ReadyMessage rMsg = context.ReadyMessages.FirstOrDefault(m => m.RestaurantChainId == RestaurantChainId);
//                if (rMsg != null)
//                    msg = rMsg.ReadyMessage1;
//            }

//            return msg;
//        }


//        public string GetVisitMessage(GetReturnGuest_Result model)
//        {
//            string msg = null;
//            using (SeatQEntities context = new SeatQEntities())
//            {
//                msg = context.SendVisitMessage((int)model.RestaurantChainId, (int)model.GuestId, (int)model.NoOfReturn).FirstOrDefault();
//            }

//            return msg;
//        }
//    }
//}
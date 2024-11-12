using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quartz;
using System.Web.Configuration;
using HastyAPI.Nexmo;
//using SeatQ.Models;
using System.Threading.Tasks;
using System.Threading;
using SeatQ.core.dal.Models;

namespace SeatQ.Jobs
{
    public class Messaging : IJob
    {

        string nexmokey = WebConfigurationManager.AppSettings["NexmoKey"];
        string nexmosecret = WebConfigurationManager.AppSettings["NexmoSecret"];
        public void Execute(IJobExecutionContext context)
        {

            //return;
            new Thread(() =>
            {
                SendMessage();
            }).Start();


        }

        public void SendMessage()
        {
            string msg = "Thank you for visiting our restaurant.";

            using (SeatQEntities ctx = new SeatQEntities())
            {
                List<GetSeatedMessageList_Result> r = ctx.GetSeatedMessageList().ToList();
                r.ForEach(x =>
                    {
                        var response = new Nexmo(nexmokey, nexmosecret).Send(x.RestaurantNumber, x.MobileNumber, msg);
                        if (response.MessageCount > 0)
                        {
                            response.Messages.ForEach(m =>
                            {
                                SeatedMessageSent s = ctx.SeatedMessageSents.FirstOrDefault(y => y.SeatedMessageSentId == x.SeatedMessageSentId);
                                s.MessageText = msg;
                                s.MessageFrom = x.RestaurantNumber;
                                s.MessageTo = x.MobileNumber;
                                s.StatusCode = (int)m.Status;
                                s.StatusMessage = m.Status.ToString();
                                s.MessageId = m.MessageID;
                                s.ErrorText = m.ErrorText;
                                s.ClientRef = m.ClientRef;
                                s.MessageSentDateTime = DateTime.UtcNow;
                                ctx.SaveChanges();
                            });
                        }
                    });

                List<GetVisitedMessageList_Result> v = ctx.GetVisitedMessageList().ToList();
                v.ForEach(x =>
                {
                    var res = new Nexmo(nexmokey, nexmosecret).Send(x.RestaurantNumber, x.MobileNumber, x.VisitMessage);
                    if (res.MessageCount > 0)
                    {
                        res.Messages.ForEach(m =>
                        {
                            VisitMessageSent s = ctx.VisitMessageSents.FirstOrDefault(y => y.VisitMessageSentId == x.VisitMessageSentId);
                            s.MessageText = x.VisitMessage;
                            s.MessageFrom = x.RestaurantNumber;
                            s.MessageTo = x.MobileNumber;
                            s.StatusCode = (int)m.Status;
                            s.StatusMessage = m.Status.ToString();
                            s.MessageId = m.MessageID;
                            s.ErrorText = m.ErrorText;
                            s.ClientRef = m.ClientRef;
                            s.MessageSentDateTime = DateTime.UtcNow;
                            ctx.SaveChanges();
                        });
                    }
                });

            }


        }
    }
}
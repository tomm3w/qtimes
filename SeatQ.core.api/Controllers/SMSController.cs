using Core.Web;
using SeatQ.core.dal.Infrastructure.Repositories;
using SeatQ.core.dal.Models;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SeatQ.core.api.Controllers
{
    public class SMSController : WebServiceBaseController
    {

        [AllowAnonymous]
        [HttpGet]
        public HttpResponseMessage inbound([FromUri]InBoundMessageModel model)
        {
            if (ModelState.IsValid && model != null)
            {
                WaitListRepository wlRep = new WaitListRepository();
                MessageRepository rep = new MessageRepository();
                GetWaitListFromReply_Result wl = null;

                if (model.text.Trim().Equals("1"))
                {
                    wl = wlRep.GetWaitListToConfirmedByMobileNumber(model.to, model.msisdn, model.message_timestamp);
                    if (wl != null)
                    {
                        wlRep.Confirmed((int)wl.WaitListId);
                    }
                }
                else if (model.text.Trim().Equals("2"))
                {
                    wl = wlRep.GetWaitListToConfirmedByMobileNumber(model.to, model.msisdn, model.message_timestamp);
                    if (wl != null)
                    {
                        wlRep.Leave((int)wl.WaitListId);
                    }
                }
                else
                {
                    GetWaitListFromGuestReply_Result wlist = wlRep.GetWaitListForGuestMessage(model.to, model.msisdn, model.message_timestamp);
                    if (wlist != null)
                    {
                        rep.SaveGuestMessage(model, (int)wlist.WaitListId);
                    }
                }
                rep.SaveReply(model, wl == null ? 0 : wl.WaitListId);
            }

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        [AllowAnonymous]
        public HttpResponseMessage deliverystatus(DeliveryReceiptModel model)
        {
            if (ModelState.IsValid && model != null)
            {
                new MessageRepository().SaveDeliveryStatus(model);
            }
            return new HttpResponseMessage(HttpStatusCode.OK);
        }


    }
}
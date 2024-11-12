using System;
using Microsoft.AspNetCore.Mvc;

namespace iVeew.core.Web
{
    public enum JSONType
    {
        Data = 0,
        Command = 1,
        Message = 2,
        UnAuthorize = 4,
        InvalidMethod = 8
    }

    public class WebServiceBaseController : Controller
    {
        protected JSONType JsonType { get; set; }
        //protected JsonRequestBehavior JsonRequestBehavior { get; set; }
        //public UserProfile AuthenticatedUser { get; set; }
        public WebServiceBaseController()
        {
            //JsonRequestBehavior = JsonRequestBehavior.AllowGet;
        }
      
        #region overrides
        /// <summary>
        /// It is overriden function of controller base class 
        /// used to write "status":"ok" at the begining of returned json.
        /// </summary>
        /// <param name="filterContext"></param>
        //protected override void OnActionExecuting(ActionExecutingContext filterContext)
        //{
            
        //    base.OnActionExecuting(filterContext);
        //    string trace = HttpContext.Request.Form["trace"];
        //    if ((trace ?? "false") != "false")
        //    {
        //        //Logger.LogHTTPRequest();
        //    }
            
        //    //JsonRequestBehavior = JsonRequestBehavior.DenyGet;
            
        //    //filterContext.HttpContext.Response.Write(@"{""status"":""ok"",""data"":");
        //    //if (Session != null && Session["sessionuser"] != null)
        //    //    user = (SessionUser)Session["sessionuser"];
        //}
        /// <summary>
        /// It is overriden function of controller base class 
        /// used to append Tokenkey at end of returned json.
        /// </summary>
        /// <param name="filterContext"></param>
        //protected override void OnResultExecuted(ResultExecutedContext filterContext)
        //{
        //    base.OnResultExecuted(filterContext);
        //   // string tokenKey = filterContext.Controller.TempData["TokenKey"] != null ? filterContext.Controller.TempData["TokenKey"].ToString() : "";
        //   // AuthenticatedUser = (User)filterContext.Controller.TempData["userprofile"];
        //    //filterContext.HttpContext.Response.Write(@",""TokenKey"":""" + tokenKey + @"""}");
            
        //}

        

        protected JsonResult GetJSON(object data)
        {
            object d = new { status = "ok", type = Enum.GetName(typeof(JSONType), JsonType).ToLower(), data = data };
            return new JsonResult(d);
        }

        //protected override void HandleUnknownAction(string actionName)
        //{
        //    //base.HandleUnknownAction(actionName);
        //    new JsonResult()
        //    {
        //        Data = new
        //        {
        //            status = "error",
        //            type = Enum.GetName(typeof(JSONType), JSONType.InvalidMethod).ToLower()
        //        },
        //        JsonRequestBehavior = System.Web.Mvc.JsonRequestBehavior.AllowGet
        //    }.ExecuteResult(ControllerContext);
        //}


        #endregion

    }
}
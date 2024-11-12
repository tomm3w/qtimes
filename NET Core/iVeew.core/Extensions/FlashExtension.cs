using System;
using System.Text;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace iVeew.Core.Extensions
{
    public static class FlashExtension
    {

        public static void FlashInfo(this Controller controller, string message)
        {
            controller.TempData["info"] = message;
        }
        public static void FlashWarning(this Controller controller, string message)
        {
            controller.TempData["warning"] = message;
        }
        public static void FlashError(this Controller controller, string message)
        {
            controller.TempData["error"] = message;
        }

        public static string Flash(this HtmlHelper helper, int ?hideAfter= 3000)
        {

            var message = "";
            var className = "";
            if (helper.ViewContext.TempData["info"] != null)
            {
                message = helper.ViewContext.TempData["info"].ToString();
                className = "info";
            }
            else if (helper.ViewContext.TempData["warning"] != null)
            {
                message = helper.ViewContext.TempData["warning"].ToString();
                className = "warning";
            }
            else if (helper.ViewContext.TempData["error"] != null)
            {
                message = helper.ViewContext.TempData["error"].ToString();
                className = "error";
            }
            var sb = new StringBuilder();
            if (!String.IsNullOrEmpty(message))
            {
                sb.AppendLine("<script>");
                sb.AppendLine("$(document).ready(function() {");
                //sb.AppendFormat("$('#flash').html('{0}');", message);
                sb.AppendFormat("$('#flash').html('{0}');", HttpUtility.HtmlEncode(message));
                sb.AppendFormat("$('#flash').toggleClass('{0}');", className);
                sb.AppendLine("$('#flash').slideDown('slow');");
                sb.AppendLine("$('#flash').click(function(){$('#flash').toggle('highlight')});");
                sb.AppendLine("setTimeout(function(){$('#flash').slideUp('highlight');},"+(hideAfter)+");");
                sb.AppendLine("});");
                sb.AppendLine("</script>");
            }
            return sb.ToString();
        }

    }
}

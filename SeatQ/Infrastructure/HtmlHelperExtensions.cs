using System.Web.Mvc;

namespace SeatQ.Infrastructure
{
	public static  class HtmlHelperExtensions
	{
		public static string GetAddonManageUrl(this HtmlHelper helper, int addonId)
		{
			var urlHelper = new UrlHelper(helper.ViewContext.RequestContext);
       
			if (addonId == 5)
			{
				var link = urlHelper.Action("Index", "PushNotification");
				TagBuilder builder = new TagBuilder("a");
				builder.Attributes["href"] = link;
				builder.SetInnerText("Go to dashboard");
				return builder.ToString();
			}
			return string.Empty;
		}
	}
}
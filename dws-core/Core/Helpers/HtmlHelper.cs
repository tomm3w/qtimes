using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Core.Helpers
{
	class FakeController : Controller
	{

	}
	public static class HtmlHelper
	{
		public static string GetHTML(string ViewPath, object model)
		{
			var context = ViewRenderer.CreateController<EmptyController>().ControllerContext;

			ViewRenderer renderer = new ViewRenderer(context);
			string html = renderer.RenderViewToString(ViewPath, model);
			return html;
		}

	}
}

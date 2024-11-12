using System;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;
using Authentication.Web;
using common.api.Infrastructure.Exeptions;

namespace common.api.Infrastructure
{
	/// <summary>
	/// Base class for all api controllers
	/// </summary>
	public class SpecificApiController : ApiController
	{
		protected IApplicationFacade Facade;
		protected ClaimsPrincipal Principal;
		public SpecificApiController(IApplicationFacade facade)
		{
			Facade = facade;

		}

		private void InitializePrincipal()
		{
			if (Principal == null && Request != null)
				Principal = Request.GetRequestContext().Principal as ClaimsPrincipal;
		}

		public bool Administrator
		{
			get
			{
				InitializePrincipal();
				return Principal.IsInRole("Administrator");
			}
		}
		public bool RegionalManager
		{
			get
			{
				InitializePrincipal();
				return Principal.IsInRole("Regional Manager");
			}
		}

		public bool BusinessAdmin
		{
			get
			{
				InitializePrincipal();
				return Principal.IsInRole("Business Admin");
			}
		}

		public bool IsUserRole
		{
			get
			{
				InitializePrincipal();
				return Principal.IsInRole("User");
			}
		}

		public string Username
		{
			get
			{
				InitializePrincipal();
				return Principal.Identity.Name;
			}
		}

		public Guid UserId
		{
			get
			{
				InitializePrincipal();
				return Security.CurrentUserIdByName(Principal.Identity.Name);
			}
		}
		protected HttpResponseMessage CreateHttpResponse(object data)
		{
			return CreateHttpResponse(data, HttpStatusCode.OK);
		}

		protected HttpResponseMessage CreateHttpResponse(HttpStatusCode statusCode)
		{
			return CreateHttpResponse(new {}, statusCode);
		}
		protected HttpResponseMessage CreateHttpResponse(object data, HttpStatusCode statusCode)
		{
			return new HttpResponseMessage(statusCode)
			{
				Content = GetJSON(data, statusCode)
			};
		}
		protected HttpResponseMessage CreateHttpResponse(string message, HttpStatusCode statusCode)
		{
			return new HttpResponseMessage(statusCode)
			{
				Content = new StringContent(message)
			};
		}

		protected HttpResponseMessage CreateHttpResponse(Exception ex)
		{
			return new HttpResponseMessage(HttpStatusCode.OK)
			{
				Content = GetErrorJSON(ex)
			};
		}

		protected HttpResponseMessage CreateHttpResponse(ApiException ex)
		{
			return new HttpResponseMessage(HttpStatusCode.OK)
			{
				Content = GetErrorJSON(ex)
			};
		}

		protected JsonContent GetJSON(object data, HttpStatusCode statusCode)
		{
			return new JsonContent(new { status = statusCode.ToString().ToLower(), type = "data", data = data });
		}

		protected JsonContent GetErrorJSON(Exception ex)
		{
			return new JsonContent(
				new
				{
					status = HttpStatusCode.InternalServerError.ToString().ToLower(),
					type = "servererror"
				});
		}

		protected JsonContent GetErrorJSON(ApiException  apiException)
		{
			return new JsonContent(
				new
				{
					status = apiException.HttpStatusCode.ToString().ToLower(),
					type = "applicationerror",
					message = apiException.Message
				});
		}
	}
}
using System;
using System.Net;

namespace common.api.Infrastructure.Exeptions
{
	public class ApiException : Exception
	{
		public HttpStatusCode HttpStatusCode { get; private set; }
		public ApiException(string message, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
			: base(message)
		{
			HttpStatusCode = statusCode;
		}
	}
}
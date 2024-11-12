using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using iVeew.common.api.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using QTimes.api.Exceptions;

namespace QTimes.api.Filters
{
    public class InvalidPostDataExceptionFilterAttribute : ExceptionFilterAttribute
	{
        public override void OnException(ExceptionContext context)
        {
			if (context.Exception is InvalidPostDataException)
			{
				var dataException = (InvalidPostDataException)context.Exception;
				var errorStates = (from error in dataException.ModelState where error.Key != "model" && error.Value.Errors.Count > 0 select error).ToArray();
				var errors = errorStates.ToDictionary<KeyValuePair<string, ModelStateEntry>, string, Array>(state => state.Key, state =>
					(from error in state.Value.Errors where !string.IsNullOrEmpty(error.ErrorMessage) select error.ErrorMessage).ToArray());

				var response = new HttpResponseMessage(HttpStatusCode.OK);
				var errorCollections = errorStates.Count() > 0 ? errorStates.First().Value.Errors : null;

				string errormessage = errorCollections != null && errorCollections.Any() ? errorCollections[0].ErrorMessage : "Please check your data!";

				response.Content = new JsonContent(
				 new
				 {
					 status = HttpStatusCode.BadRequest.ToString().ToLower(),
					 type = "applicationerror",
					 message = errormessage,
					 errors = errors
				 });
				context.Result = new JsonResult(response);
			}
		//	base.OnException(context);
        }
	}
}
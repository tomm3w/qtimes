using System;
using System.Collections.Generic;
using System.Linq;
using iVeew.Core.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace iVeew.Core.Attributes
{
    public class WebserviceExecptionAttribute : ActionFilterAttribute, IExceptionFilter
    {
        /// <summary>
        /// This is callled once any error occured in the webservice.
        /// It generated error json.
        /// </summary>
        /// <param name="filterContext"></param>
        public void OnException(ExceptionContext filterContext)
        {
            //Don't interfer if the exception is already handled

            if (filterContext.ExceptionHandled)
            {
                return;
            }
            System.Exception ex = filterContext.Exception;
            if (typeof(InvalidPostDataException) == filterContext.Exception.GetType())
            {
                InvalidPostDataException modelException = (InvalidPostDataException)filterContext.Exception;
                var errorStates = (from error in modelException.ModelState where error.Value.Errors.Count > 0 select error).ToArray();
                bool hasError = errorStates.Count() > 0;
                Dictionary<string, Array> errors = new Dictionary<string, Array>();
                List<string> errorList = new List<string>();
                foreach (var state in errorStates)
                {
                    errors.Add(state.Key, (from error in state.Value.Errors select error.ErrorMessage).ToArray());
                }
                filterContext.Result =
                    new JsonResult
                    (
                        new
                        {
                            status = "error",
                            ErrorType = (hasError ? "validation" : "NotImplemented"),
                            ErrorMessage = (hasError) ? errorStates.First().Value.Errors[0].ErrorMessage : "Coming soon",
                            //ErrorMessage = "data is not valid",
                            Errors = errors,
                            ErrorCode = modelException.ErrorCode
                        }
                        // JsonRequestBehavior = JsonRequestBehavior.AllowGet
                    );
            }
            else if (typeof(CoreException) == filterContext.Exception.GetType())
            {
                CoreException exception = (CoreException)filterContext.Exception;
                filterContext.Result =
                    new JsonResult
                    (
                        new
                        {
                            status = "error",
                            ErrorType = Enum.GetName(typeof(ErrorType), exception.ErrorType),
                            ErrorMessage = exception.ErrorCode == ErrorCode.AccessDenied ? "Access Denied" : exception.ErrorMessage,
                            ErrorCode = exception.ErrorCode
                        }
                        // JsonRequestBehavior = JsonRequestBehavior.AllowGet
                    );
            }
            else
            {
                filterContext.Result =
                    new JsonResult
                    (
                        new
                        {
                            status = "error",
                            ErrorType = "system",
                            ErrorCode = ErrorCode.UnKnownError,
                            ErrorMessage = ex.Message,
                            ExceptionDetail = ex.StackTrace,
                            InnerException = InnerException(ex.InnerException)
                        } //,
                        //JsonRequestBehavior = JsonRequestBehavior.AllowGet
                    );

            }
            //Logger.Log(ex);
            //Advise subsequent exception filters not to interfere
            //and stop ASP.NET from producing a "yellow screen of death"
            filterContext.ExceptionHandled = true;

            //Erase any output already generated
            // filterContext.HttpContext.Response.Clear();

        }


        private object InnerException(Exception ex)
        {

            if (ex == null)
            {
                return null;
            }
            var data = new
            {
                Messsage = ex.Message,
                InnerException = InnerException(ex.InnerException)
            };
            return data;
        }
    }

}
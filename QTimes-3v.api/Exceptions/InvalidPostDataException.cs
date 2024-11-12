using common.api.Infrastructure.Exeptions;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace QTimes.api.Exceptions
{
    public class InvalidPostDataException : ApiException
    {
        public InvalidPostDataException(ModelStateDictionary modelState):base(null, System.Net.HttpStatusCode.BadRequest)
        {
            ModelState = modelState;
        }
        public ModelStateDictionary ModelState { get; set; }
    }
}
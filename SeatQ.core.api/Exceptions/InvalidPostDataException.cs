using System.Web.Http.ModelBinding;
using common.api.Infrastructure.Exeptions;

namespace SeatQ.core.api.Exceptions
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
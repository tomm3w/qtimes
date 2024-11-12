using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace iVeew.Core.Exceptions
{
    public class InvalidPostDataException : System.Exception
    {
        public InvalidPostDataException()
            : base()
        {

        }
        public InvalidPostDataException(ModelStateDictionary modelState)
        {
            ModelState = modelState;
            ErrorCode = ErrorCode.UnKnownError;
        }
        
        public ErrorCode ErrorCode { get; set; }
        public ModelStateDictionary ModelState { get; set; }
    }
}
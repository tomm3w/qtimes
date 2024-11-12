using System;

namespace iVeew.Core.Helpers
{
    public class ExceptionHelper
    {
        public static string GetInnerExpectionMessage(Exception ex)
        {
            string retVal = ex.Message;
            if (ex.InnerException != null)
            {
                Exception innerEx = ex.InnerException;
                retVal = GetInnerExpectionMessage(innerEx);
            }            
            return retVal;
        }
    }
}

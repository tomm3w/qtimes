using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Helpers
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

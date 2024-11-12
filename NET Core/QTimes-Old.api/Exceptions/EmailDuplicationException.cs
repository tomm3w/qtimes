using System.Net;
using common.api.Infrastructure.Exeptions;

namespace QTimes.api.Exceptions
{
    public class EmailDuplicationException : ApiException
    {
        public EmailDuplicationException()
            : base("User with same Email exists in database.", HttpStatusCode.Conflict)
        {

        }
    }
}
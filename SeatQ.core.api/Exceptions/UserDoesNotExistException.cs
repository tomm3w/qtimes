using System.Net;
using common.api.Infrastructure.Exeptions;

namespace SeatQ.core.api.Exceptions
{
    public class UserDoesNotExistException : ApiException
    {
        public UserDoesNotExistException() : base("User does not exists in database.", HttpStatusCode.NotFound)
        {

        }
    }
}
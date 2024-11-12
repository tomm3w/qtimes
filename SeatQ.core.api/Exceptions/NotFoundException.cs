using System.Net;
using common.api.Infrastructure.Exeptions;

namespace SeatQ.core.api.Exceptions
{
    public class NotFoundException : ApiException
    {
        public NotFoundException() : base("Object Not Found.", HttpStatusCode.NotFound)
        {

        }

        public NotFoundException(string message) : base(message, HttpStatusCode.NotFound)
        {

        }
    }
}
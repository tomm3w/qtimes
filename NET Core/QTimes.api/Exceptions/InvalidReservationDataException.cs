using System.Net;
using common.api.Infrastructure.Exeptions;

namespace QTimes.api.Exceptions
{
    public class InvalidReservationDataException : ApiException
    {
        public InvalidReservationDataException(string message) : base(message, HttpStatusCode.BadRequest)
        {

        }
    }
}
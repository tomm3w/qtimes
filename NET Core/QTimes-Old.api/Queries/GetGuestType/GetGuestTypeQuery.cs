using iVeew.common.api.Queries;
using IVeew.common.dal;
using QTimes.core.dal.Models;
using System.Linq;

namespace QTimes.api.Queries.GetGuestType
{
    public class GetGuestTypeQuery : IQuery<GetGuestTypeResponse, GetGuestTypeRequest>
    {
        private readonly IGenericRepository<QTimesContext, GuestType> _guestType;

        public GetGuestTypeQuery(IGenericRepository<QTimesContext, GuestType> guestType)
        {
            _guestType = guestType;
        }

        public GetGuestTypeResponse Handle(GetGuestTypeRequest request)
        {
            var query = _guestType.GetAll().ToList();
            return new GetGuestTypeResponse(query);
        }
    }
}
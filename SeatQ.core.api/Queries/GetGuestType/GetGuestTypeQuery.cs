using common.api.Queries;
using common.dal;
using SeatQ.core.dal.Models;
using System.Linq;

namespace SeatQ.core.api.Queries.GetGuestType
{
    public class GetGuestTypeQuery : IQuery<GetGuestTypeResponse, GetGuestTypeRequest>
    {
        private readonly IGenericRepository<SeatQEntities, GuestType> _guestType;

        public GetGuestTypeQuery(IGenericRepository<SeatQEntities, GuestType> guestType)
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
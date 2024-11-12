using common.dal;
using SeatQ.core.dal.Models;


namespace SeatQ.core.dal.Repositories
{
    class ReservationBusinessRepo : GenericRepository<SeatQEntities, ReservationBusiness>, IReservationBusinessRepo
    {
    }
}

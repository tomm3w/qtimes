using common.dal;
using SeatQ.core.dal.Models;

namespace SeatQ.core.dal.Repositories
{
    public class ConcertReservationRepo : GenericRepository<SeatQEntities, ConcertReservation>, IConcertReservationRepo
    {
    }
}

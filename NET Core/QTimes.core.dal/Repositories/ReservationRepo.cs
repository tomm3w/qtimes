using iVeew.common.dal;
using QTimes.core.dal.Models;


namespace SeatQ.core.dal.Repositories
{
    public class ReservationRepo : GenericRepository<QTimesContext, Reservation>, IReservationRepo
    {
    }
}

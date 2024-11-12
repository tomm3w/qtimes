using iVeew.common.dal;
using QTimes.core.dal.Models;
using QTimes.core.dal.Repositories;

namespace SeatQ.core.dal.Repositories
{
    public class DeliveryStatusRepo : GenericRepository<QTimesContext, DeliveryStatus>, IDeliveryStatusRepo
    {
    }
}

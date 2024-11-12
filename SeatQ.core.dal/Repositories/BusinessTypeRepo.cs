using common.dal;
using SeatQ.core.dal.Models;

namespace SeatQ.core.dal.Repositories
{
    public class BusinessTypeRepo : GenericRepository<SeatQEntities, BusinessType>, IBusinessTypeRepo
    {
    }
}

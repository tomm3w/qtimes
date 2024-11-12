using common.dal;
using SeatQ.core.dal.Models;

namespace SeatQ.core.dal.Repositories
{
    public class LoyaltyMessageRepo : GenericRepository<SeatQEntities, LoyaltyMessage>, ILoyaltyMessageRepo
    {
    }
}

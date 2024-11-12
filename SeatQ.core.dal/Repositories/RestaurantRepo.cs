using common.dal;
using SeatQ.core.dal.Models;

namespace SeatQ.core.dal.Repositories
{
    public class RestaurantRepo : GenericRepository<SeatQEntities, Restaurant>, IRestaurantRepo
    {
    }
}

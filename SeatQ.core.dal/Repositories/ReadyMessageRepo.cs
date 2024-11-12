using common.dal;
using SeatQ.core.dal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeatQ.core.dal.Repositories
{
    public class ReadyMessageRepo : GenericRepository<SeatQEntities, ReadyMessage>, IReadyMessageRepo
    {
    }
}

﻿using iVeew.common.dal;
using QTimes.core.dal.Models;
namespace SeatQ.core.dal.Repositories
{
    public class ConcertEventRepo : GenericRepository<QTimesContext, ConcertEvent>, IConcertEventRepo
    {
    }
}
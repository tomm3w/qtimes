using common.api.Queries;
using System;
using System.Collections.Generic;

namespace SeatQ.core.api.Queries.Reservation.GetBusinessAccounts
{
    public class GetBusinessAccountsResponse : IQueryResponse
    {
        public List<BusinessAccount> BusinessAccounts { get; set; }
    }

    public class BusinessAccount
    {
        public Guid UserId { get; set; }
        public Guid ReservationBusinessId { get; set; }
        public Guid BusinessId { get; set; }
        public string BusinessName { get; set; }
        public string BusinessType { get; set; }
        public string LogoPath { get; set; }
        public string LogoFullPath { get; set; }
        public Guid ConcertId { get; set; }
    }
}
using iVeew.common.api.Queries;

namespace QTimes.api.Queries.Reservation.GetAccount
{
    public class GetAccountResponse : IQueryResponse
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string BusinessName { get; set; }
        public string FullName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string TimezoneOffset { get; set; }
        public string TimezoneOffsetValue { get; set; }
    }
}
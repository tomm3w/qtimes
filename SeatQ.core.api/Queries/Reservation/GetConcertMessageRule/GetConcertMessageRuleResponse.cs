using common.api.Queries;
using System;
using System.Collections.Generic;

namespace SeatQ.core.api.Queries.Reservation.GetConcertMessageRule
{
    public class GetConcertMessageRuleResponse : IQueryResponse
    {
        public List<ConcertMessageRules> Model { get; set; }
    }

    public class ConcertMessageRules
    {
        public int Id { get; set; }
        public Nullable<System.Guid> ConcertId { get; set; }
        public string MessageType { get; set; }
        public Nullable<short> Value { get; set; }
        public string ValueType { get; set; }
        public string BeforeAfter { get; set; }
        public string InOut { get; set; }
        public string Message { get; set; }
    }
}
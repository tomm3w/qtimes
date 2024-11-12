using iVeew.common.api.Queries;
using System;
using System.Collections.Generic;

namespace QTimes.api.Queries.Reservation.GetConcertEventMessageRule
{
    public class GetConcertEventMessageRuleResponse : IQueryResponse
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
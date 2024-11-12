using iVeew.common.api.Queries;
using QTimes.core.dal.Models;
using System;
using System.Collections.Generic;

namespace QTimes.api.Queries.Reservation.GetReservationMessageRule
{
    public class GetReservationMessageRuleResponse : IQueryResponse
    {
        public List<MessageRules> Model { get; set; }
    }

    public class MessageRules
    {
        public int Id { get; set; }
        public Nullable<System.Guid> ReservationBusinessId { get; set; }
        public string MessageType { get; set; }
        public Nullable<short> Value { get; set; }
        public string ValueType { get; set; }
        public string BeforeAfter { get; set; }
        public string InOut { get; set; }
        public string Message { get; set; }
    }
}
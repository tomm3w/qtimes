using common.api.Queries;
using Core.Extensions;
using System;
using System.Collections.Generic;

namespace SeatQ.core.api.Queries.Reservation.GetMessages
{
    public class GetMessagesResponse : IQueryResponse
    {
        public List<Messages> Model { get; set; }
    }

    public class Messages
    {
        public int Id { get; set; }
        public Nullable<int> ReservationId { get; set; }
        public string MessageSent { get; set; }
        public Nullable<System.DateTime> MessageSentDateTime { get; set; }
        public Nullable<System.Guid> MessageSentBy { get; set; }
        public string MessageReplied { get; set; }
        public string GuestName { get; set; }
        public Nullable<System.DateTime> MessageRepliedDateTime { get; set; }
        public string MessageSentDateTimeUniversal
        {
            get
            {
                if (MessageSentDateTime != null)
                    return MessageSentDateTime.ToUniversalTimeString();
                else
                    return "";
            }
        }
        public string MessageRepliedDateTimeUniversal
        {
            get
            {
                if (MessageRepliedDateTime != null)
                    return MessageRepliedDateTime.ToUniversalTimeString();
                else
                    return "";
            }
        }
    }
}
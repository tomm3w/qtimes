﻿using common.api.Queries;
using Core.Extensions;
using System;
using System.Collections.Generic;

namespace SeatQ.core.api.Queries.Reservation.GetConcertMessages
{
    public class GetConcertMessagesResponse : IQueryResponse
    {
        public List<ConcertMessages> Model { get; set; }
    }

    public class ConcertMessages
    {
        public int Id { get; set; }
        public Nullable<int> ConcertReservationId { get; set; }
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
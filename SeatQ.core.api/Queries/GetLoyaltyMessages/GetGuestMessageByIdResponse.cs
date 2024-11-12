using common.api.Queries;
using SeatQ.core.dal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SeatQ.core.api.Queries.GetLoyaltyMessages
{
    public class GetLoyaltyMessageByIdResponse : IQueryResponse
    {
        public List<LoyaltyMessage> LoyaltyMessage { get; private set; }
        public GetLoyaltyMessageByIdResponse(List<LoyaltyMessage> loyaltyMessage)
		{
            LoyaltyMessage = loyaltyMessage;
		}
    }
}
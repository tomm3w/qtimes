using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeatQ.core.common.Dto
{
    public class RestaurantDto
    {
        public RestaurantDto(int restaurantId, string businessName, string logoPath, bool isconfirmed, List<RestaurantChainDto> restaurantChain)
        {
            RestaurantId = restaurantId;
            BusinessName = businessName;
            LogoPath = logoPath;
            isConfirmed = isconfirmed;
            RestaurantChain = restaurantChain;
        }
        public int RestaurantId { get; private set; }
        public string BusinessName { get; private set; }
        public string LogoPath { get; private set; }
        public bool isConfirmed { get; private set; }
        public List<RestaurantChainDto> RestaurantChain { get; private set; }
    }
}

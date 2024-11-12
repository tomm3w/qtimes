using System;

namespace QTimes.core.common.Dto
{
    public class RestaurantChainDto
    {
        public RestaurantChainDto(int restaurantId, int restaurantChainId, Guid userId, string restaurantNumber,
            string fullName, string email, string address1, string address2, string city, string state, string zip, string country, string phone,
            RestaurantDto restaurant
            )
        {
            RestaurantId = restaurantId;
            RestaurantChainId = restaurantChainId;
            UserId = userId;
            RestaurantNumber = restaurantNumber;
            FullName = fullName;
            Email = email;
            Address1 = address1;
            Address2 = address2;
            City = city;
            State = state;
            Zip = zip;
            Country = country;
            Phone = phone;
            Restaurant = restaurant;
        }
        public int RestaurantId { get; private set; }

        public int RestaurantChainId { get; private set; }

        public Guid UserId { get; private set; }

        public string FullName { get; private set; }

        public string Email { get; private set; }

        public string Address1 { get; private set; }

        public string Address2 { get; private set; }

        public string City { get; private set; }

        public string State { get; private set; }

        public string Zip { get; private set; }

        public string Country { get; private set; }

        public string Phone { get; private set; }

        public string RestaurantNumber { get; private set; }

        public RestaurantDto Restaurant { get; private set; }
    }
}

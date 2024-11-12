using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeatQ.core.common.Dto
{
    public class UserDto
    {
        public UserDto(Guid id, string username, string email, string role, Guid roleId, bool isActive, bool isDeleted, DateTime createDate, DateTime? lastAccessDateTime, int? restaurantChainId, int? restaurantId, string restaurantNumber)
        {
            RoleId = roleId;
            Role = role;
            Email = email;
            Username = username;
            Id = id;
            isUserActive = isActive;
            isUserDeleted = isDeleted;
            LastAccessDateTime = lastAccessDateTime;
            CreateDate = createDate;
            RestaurantChainId = restaurantChainId;
            RestaurantId = restaurantId;
            RestaurantNumber = restaurantNumber;
        }

        public Guid Id { get; private set; }
        public string Username { get; private set; }

        public string Email { get; private set; }

        public string Role { get; private set; }

        public Guid RoleId { get; private set; }

        public bool isUserActive { get; private set; }
        public bool isUserDeleted { get; private set; }

        public DateTime? LastAccessDateTime { get; private set; }

        public DateTime CreateDate { get; private set; }

        public int? RestaurantChainId { get; private set; }

        public int? RestaurantId { get; private set; }

        public string RestaurantNumber { get; private set; }
    }
}

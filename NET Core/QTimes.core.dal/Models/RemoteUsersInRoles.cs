using System;
using System.Collections.Generic;

namespace QTimes.core.dal.Models
{
    public partial class RemoteUsersInRoles
    {
        public Guid UserId { get; set; }
        public Guid RoleId { get; set; }
    }
}

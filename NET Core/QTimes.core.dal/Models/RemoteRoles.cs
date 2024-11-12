using System;
using System.Collections.Generic;

namespace QTimes.core.dal.Models
{
    public partial class RemoteRoles
    {
        public Guid RoleId { get; set; }
        public Guid ApplicationId { get; set; }
        public string RoleName { get; set; }
        public string Description { get; set; }
    }
}

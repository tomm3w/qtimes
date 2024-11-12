using System;
using System.Collections.Generic;

namespace QTimes.core.dal.Models
{
    public partial class RemoteUsers
    {
        public Guid UserId { get; set; }
        public Guid ApplicationId { get; set; }
        public string UserName { get; set; }
        public bool IsAnonymous { get; set; }
        public DateTime LastActivityDate { get; set; }
        public Guid? ParentUserId { get; set; }
    }
}

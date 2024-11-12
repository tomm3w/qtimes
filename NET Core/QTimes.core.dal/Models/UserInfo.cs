using System;
using System.Collections.Generic;

namespace QTimes.core.dal.Models
{
    public partial class UserInfo
    {
        public Guid UserId { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? LastAccessDateTime { get; set; }
        public bool? IsPasswordChanged { get; set; }
        public Guid? ReservationBusinessId { get; set; }
        public bool? IsConfirmed { get; set; }
        public Guid? ConfirmationToken { get; set; }

        public virtual ReservationBusiness ReservationBusiness { get; set; }
    }
}
